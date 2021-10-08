using ancient.game.client.renderer.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ancient.game.input;
using ancient.game.client.gui.component;
using ancient.game.client.utils;
using ancient.game.client.gui.component.button;
using ancient.game.client.network;
using ancientlib.game.network.packet.client.player;
using ancient.game.entity.player;

namespace ancient.game.client.gui
{
    public class GuiCharacterCreation : GuiCameraScroller
    {
        private static float MIN_DISTANCE = 3;
        private static float MAX_DISTANCE = 7;
        private static float SCROLL_VALUE = 0.4F;

        private GuiText characterCreationText;
        private GuiTextBox nickname;
        private GuiOptionSwitch hairSwitcher;
        private GuiOptionSwitch eyesSwitcher;
        private GuiButton createCharacter;

        private GuiColorRGB hairColor;

        public GuiText errorText;

        private EntityPlayer character;

        public GuiCharacterCreation(GuiManager guiManager) : base(guiManager, "character_creation", MIN_DISTANCE, MAX_DISTANCE, SCROLL_VALUE)
        {
            this.character = new EntityPlayer(null);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.characterSelection;

            this.characterCreationText = new GuiText("Character Creation", 4, 5);
            this.characterCreationText.SetOutline(1);
            this.characterCreationText.Centralize();
            this.characterCreationText.SetY(0.05F);
            this.components.Add(characterCreationText);

            // Nickname
            nickname = new GuiTextBox();
            nickname.Centralize();
            nickname.SetX(0.025F);
            nickname.SetY(0.5F);
            nickname.SetHeight(30);

            nickname.SetGuiText(new GuiText("Nickname:", 2, 2).SetColor(Color.White));
            nickname.CentralizeText();
            nickname.GetGuiText().AddY(GuiUtils.GetRelativeYFromY(-nickname.GetGuiText().GetHeight() - 10));
            nickname.GetGuiText().SetOutline(1);

            nickname.GetTextBox().SetSize(2);
            nickname.GetTextBox().SetOutline(1);

            this.components.Add(nickname);

            // Hair Switcher
            hairSwitcher = new GuiOptionSwitch();
            hairSwitcher.AddOption((byte)0);
            hairSwitcher.AddOption((byte)1);

            hairSwitcher.Centralize();
            hairSwitcher.SetX(0.025F);
            hairSwitcher.SetY(nickname.GetY() + GuiUtils.GetRelativeYFromY(nickname.GetHeight() + 30));
            hairSwitcher.SetHeight(30);

            hairSwitcher.SetGuiText(new GuiText("Hair:", 2, 2).SetColor(Color.White));
            hairSwitcher.CentralizeText();
            hairSwitcher.GetGuiText().AddY(GuiUtils.GetRelativeYFromY(-hairSwitcher.GetGuiText().GetHeight() - 10));
            hairSwitcher.GetGuiText().SetOutline(1);

            hairSwitcher.OptionChanged += new OptionChangedEventHandler(OnHairChanged);
            this.components.Add(hairSwitcher);

            // Eyes Switcher
            eyesSwitcher = new GuiOptionSwitch();
            eyesSwitcher.AddOption((byte)0);
            eyesSwitcher.AddOption((byte)1);
            eyesSwitcher.AddOption((byte)2);
            eyesSwitcher.AddOption((byte)3);
            eyesSwitcher.AddOption((byte)4);
            eyesSwitcher.AddOption((byte)5);

            eyesSwitcher.Centralize();
            eyesSwitcher.SetX(0.025F);
            eyesSwitcher.SetY(hairSwitcher.GetY() + GuiUtils.GetRelativeYFromY(hairSwitcher.GetHeight() + 30));
            eyesSwitcher.SetHeight(30);

            eyesSwitcher.SetGuiText(new GuiText("Eyes:", 2, 2).SetColor(Color.White));
            eyesSwitcher.CentralizeText();
            eyesSwitcher.GetGuiText().AddY(GuiUtils.GetRelativeYFromY(-eyesSwitcher.GetGuiText().GetHeight() - 10));
            eyesSwitcher.GetGuiText().SetOutline(1);

            eyesSwitcher.OptionChanged += new OptionChangedEventHandler(OnEyesChanged);
            this.components.Add(eyesSwitcher);

            createCharacter = new GuiButton();
            createCharacter.ButtonClickEvent += new ButtonClickEventHandler(OnCharacterCreation);
            createCharacter.Centralize();
            createCharacter.SetY(0.85F);
            createCharacter.SetGuiText(new GuiText("Create", 3, 3).SetOutline(1));
            createCharacter.CentralizeText();
            this.components.Add(createCharacter);

            this.hairColor = new GuiColorRGB(0.5F, 0.5F);
            this.hairColor.TextChangedEvent += new TextChangedEventHandler(OnHairColorChanged);
            this.components.Add(hairColor);

            this.errorText = new GuiText();
            this.errorText.SetX(this.nickname.GetX());
            this.errorText.SetY(this.nickname.GetY() - 0.1F);
            this.errorText.SetColor(Color.Red);
            this.components.Add(errorText);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.backgroundColor = guiManager.backgroundColor;
            base.Draw(spriteBatch);
        }

        public override void Update(MouseState mouseState)
        {
            camera.SetPitch(character.GetHeadPitch());
            base.Update(mouseState);
        }

        public override void Draw3D()
        {
            base.Draw3D();
            Ancient.ancient.world.GetRenderer().ResetGraphics(backgroundColor);
            WorldRenderer.currentEffect.Parameters["ShadowsEnabled"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            EntityRenderers.renderPlayer.Draw(character);
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }

        protected override void UpdateCameraDistance()
        {
            if (InputHandler.IsScrollingUp())
                this.distance = MathHelper.Clamp(distance - scrollValue, minDistance, maxDistance);
            else if (InputHandler.IsScrollingDown())
                this.distance = MathHelper.Clamp(distance + scrollValue, minDistance, maxDistance);

            camera.SetDistance(distance);
        }

        private void OnHairChanged(object sender, EventArgs e)
        {
            character.SetHairID((byte)hairSwitcher.GetSelectedOption());
        }

        private void OnHairColorChanged(object sender, EventArgs e)
        {
            character.SetHairColor(hairColor.GetColorValue());
        }

        private void OnEyesChanged(object sender, EventArgs e)
        {
            character.SetEyesID((byte)eyesSwitcher.GetSelectedOption());
        }

        private void OnCharacterCreation(object sender, EventArgs e)
        {
            if (!IsNicknameLegal())
                return;

            character.SetName(nickname.GetTextBox().GetText());

            if (!NetClient.instance.IsConnected())
            {
                guiManager.DisplayGui(guiManager.ingame);
                Ancient.ancient.player = character;
                Ancient.ancient.SpawnPlayer();
            }
            else
                NetClient.instance.SendPacket(new PacketCreateCharacter(character));
        }

        private bool IsNicknameLegal()
        {
            string name = nickname.GetTextBox().GetText();

            return name.Length >= 2 && name.Length <= 16;
        }

        public override void OnDisplay(Gui lastGui)
        {
            base.OnDisplay(lastGui);

            this.nickname.GetTextBox().SetText("");
            this.hairSwitcher.SetOptionIndex(0);
            this.eyesSwitcher.SetOptionIndex(0);

            this.errorText.SetText("");

            character.SetRotation(MathHelper.Pi, 0, 0);
            character.SetHeadRotation(MathHelper.Pi, 0);
        }

        public EntityPlayer GetCharacter()
        {
            return new EntityPlayer(null, character);
        }
    }
}
