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

        public GuiCharacterCreation(GuiManager guiManager) : base(guiManager, "character_creation", MIN_DISTANCE, MAX_DISTANCE, SCROLL_VALUE)
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.mainMenu;

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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.backgroundColor = guiManager.backgroundColor;
            base.Draw(spriteBatch);
        }

        public override void Draw3D()
        {
            Ancient.ancient.world.GetRenderer().ResetGraphics(backgroundColor);
            WorldRenderer.effect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.effect.Parameters["View"].SetValue(camera.GetViewMatrix(0, Ancient.ancient.player.GetHeadPitch()));
            WorldRenderer.effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
            EntityRenderers.GetRenderEntityFromEntity(Ancient.ancient.player).Draw(Ancient.ancient.player);
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
            Ancient.ancient.player.SetHairID((byte)hairSwitcher.GetSelectedOption());
        }

        private void OnHairColorChanged(object sender, EventArgs e)
        {
            Ancient.ancient.player.SetHairColor(hairColor.GetColorValue());
        }

        private void OnEyesChanged(object sender, EventArgs e)
        {
            Ancient.ancient.player.SetEyesID((byte)eyesSwitcher.GetSelectedOption());
        }

        private void OnCharacterCreation(object sender, EventArgs e)
        {
            if (!IsNicknameLegal())
                return;

            Ancient.ancient.player.SetName(nickname.GetTextBox().GetText());

            if (!NetClient.instance.IsConnected())
            {
                guiManager.DisplayGui(guiManager.ingame);
                Ancient.ancient.SpawnPlayer();
            }
            else
                NetClient.instance.SendPacket(new PacketCharacterCreation(Ancient.ancient.player));
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

            Ancient.ancient.player.SetRotation(MathHelper.Pi, 0, 0);
            Ancient.ancient.player.SetHeadRotation(MathHelper.Pi, 0);
        }
    }
}
