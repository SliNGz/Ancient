using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancientlib.game.utils;
using ancient.game.client.utils;
using ancient.game.client.sound;
using ancientlib.game.constants;

namespace ancient.game.client.gui
{
    public class GuiMainMenu : GuiMenuBackground
    {
        private float textHue;
        private GuiText mainMenuText;

        private GuiButtonText playButton;
        private GuiButtonText optionsButton;
        private GuiButtonText quitButton;

        private GuiText version;

        public GuiMainMenu(GuiManager guiManager) : base(guiManager, "main_menu")
        {
            this.textHue = 360;
        }

        public override void Initialize()
        {
            base.Initialize();

            InitializeMainMenuText();

            this.playButton = new GuiButtonText(new GuiText("Play").SetOutline(1));
            this.playButton.ButtonClickEvent += new ButtonClickEventHandler(OnPlayButtonClicked);
            this.playButton.Centralize();
            this.playButton.CentralizeText();
            this.components.Add(playButton);

            this.optionsButton = new GuiButtonText(new GuiText("Options").SetOutline(1));
            this.optionsButton.ButtonClickEvent += new ButtonClickEventHandler(OnOptionsButtonClicked);
            this.optionsButton.Centralize();
            this.optionsButton.AddY(GuiUtils.GetRelativeYFromY(optionsButton.GetHeight() + 20));
            this.optionsButton.CentralizeText();
            this.components.Add(optionsButton);

            this.quitButton = new GuiButtonText(new GuiText("Quit").SetOutline(1));
            this.quitButton.ButtonClickEvent += new ButtonClickEventHandler(OnQuitButtonClicked);
            this.quitButton.Centralize();
            this.quitButton.SetY(optionsButton.GetY());
            this.quitButton.AddY(GuiUtils.GetRelativeYFromY(quitButton.GetHeight() + 20));
            this.quitButton.CentralizeText();
            this.components.Add(quitButton);

            this.version = new GuiText("Version " + GameConstants.GAME_VERSION, 2, 2);
            this.version.SetX(GuiUtils.GetRelativeXFromX(3));
            this.version.SetY(1 - GuiUtils.GetRelativeYFromY(version.GetHeight() + 3));
            this.version.SetColor(Color.White);
            this.version.SetOutline(1);
            this.components.Add(version);
        }

        private void InitializeMainMenuText()
        {
            this.mainMenuText = new GuiText(GameConstants.GAME_NAME, 12, 7);
            this.mainMenuText.Centralize();
            this.mainMenuText.SetY(0.05F);
            this.mainMenuText.SetOutline(3);
            this.components.Add(mainMenuText);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            textHue -= 0.25F;

            if (textHue <= 0)
                textHue = 360;

            this.mainMenuText.SetColor(Utils.HSVToRGB(textHue, 1, 1));
        }

        public override void OnDisplay(Gui lastGui)
        {
            base.OnDisplay(lastGui);

            if (Ancient.ancient.world != null)
                Ancient.ancient.OnLeaveWorld();
        }

        public static void OnPlayButtonClicked(object sender, EventArgs e)
        {
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.playMenu);
        }

        public static void OnOptionsButtonClicked(object sender, EventArgs e)
        {
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.options);
        }

        public static void OnQuitButtonClicked(object sender, EventArgs e)
        {
            Ancient.ancient.Exit();
        }
    }
}
