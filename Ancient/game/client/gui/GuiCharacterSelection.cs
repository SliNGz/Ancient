using ancient.game.client.camera;
using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using ancient.game.client.renderer.entity;
using ancient.game.entity.player;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancient.game.input;
using ancientlib.game.user;
using ancient.game.client.network;
using ancientlib.game.network.packet.client.player;

namespace ancient.game.client.gui
{
    public class GuiCharacterSelection : GuiCameraScroller
    {
        private static float MIN_DISTANCE = 3;
        private static float MAX_DISTANCE = 7;
        private static float SCROLL_VALUE = 0.4F;

        private GuiText characterSelectionText;

        private GuiButton characterCreationButton;
        private GuiButton characterSelectButton;

        public CharactersArray charactersArray;

        public EntityPlayer character;
        public int characterIndex;
        private GuiText characterName;

        public GuiCharacterSelection(GuiManager guiManager) : base(guiManager, "character_selection", MIN_DISTANCE, MAX_DISTANCE, SCROLL_VALUE)
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.mainMenu;

            this.characterSelectionText = new GuiText("Character Selection", 4, 5);
            this.characterSelectionText.SetOutline(1);
            this.characterSelectionText.Centralize();
            this.characterSelectionText.SetY(0.05F);
            this.components.Add(characterSelectionText);

            this.characterCreationButton = new GuiButton();
            this.characterCreationButton.ButtonClickEvent += CharacterCreationButton_ButtonClickEvent;
            this.characterCreationButton.Centralize();
            this.characterCreationButton.SetX(0.05F);
            this.characterCreationButton.SetGuiText(new GuiText("Create Character", 2, 1));
            this.characterCreationButton.CentralizeText();
            this.components.Add(characterCreationButton);

            this.characterSelectButton = new GuiButton();
            this.characterSelectButton.ButtonClickEvent += CharacterSelectionButton_ButtonClickEvent;
            this.characterSelectButton.Centralize();
            this.characterSelectButton.AddY(0.35F);
            this.characterSelectButton.SetGuiText(new GuiText("Play", 2, 1));
            this.characterSelectButton.CentralizeText();
            this.components.Add(characterSelectButton);

            this.characterName = new GuiText().SetOutline(1);
            this.characterName.Centralize();
            this.characterName.AddY(-0.15F);
            this.components.Add(characterName);

            //this.charactersArray = Ancient.ancient.user.GetCharactersArray();
            this.character = Ancient.ancient.player;
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            //this.charactersArray = Ancient.ancient.user.GetCharactersArray();

            if (charactersArray.Count == 0)
                return;

            int add = 0;

            if (InputHandler.IsKeyPressed(Keys.Right))
                add += 1;
            if (InputHandler.IsKeyPressed(Keys.Left))
                add += -1;

            if (add != 0)
            {
                characterIndex = ((characterIndex + add) % charactersArray.Count + charactersArray.Count) % charactersArray.Count;
                character = charactersArray.GetCharacter(characterIndex);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.backgroundColor = guiManager.backgroundColor;

            if (character != null && characterName.GetText() != character.GetName())
            {
                this.characterName.SetText(character.GetName());
                this.characterName.Centralize();
                this.characterName.AddY(-0.15F);
            }

            base.Draw(spriteBatch);
        }

        public override void Draw3D()
        {
            if (character == null)
                return;

            Ancient.ancient.world.GetRenderer().ResetGraphics(backgroundColor);
            WorldRenderer.currentEffect.Parameters["FogEnabled"].SetValue(false);

            camera.SetPitch(character.GetHeadPitch());

            WorldRenderer.currentEffect.Parameters["View"].SetValue(camera.GetViewMatrix());
            WorldRenderer.currentEffect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            WorldRenderer.currentEffect.Parameters["ShadowsEnabled"].SetValue(false);

            EntityRenderers.renderPlayer.Draw(character);

            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }

        private void CharacterCreationButton_ButtonClickEvent(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.characterCreation);
        }

        private void CharacterSelectionButton_ButtonClickEvent(object sender, EventArgs e)
        {
            NetClient.instance.SendPacket(new PacketSelectCharacter(characterIndex));
        }
    }
}
