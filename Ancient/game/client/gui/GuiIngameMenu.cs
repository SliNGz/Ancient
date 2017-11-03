using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using ancient.game.client.network;
using ancient.game.client.utils;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui
{
    public class GuiIngameMenu : Gui
    {
        private GuiButtonText backButton;
        private GuiButtonText optionsButton;
        private GuiButtonText mainMenuButton;
        private GuiButtonText quitButton;

        public GuiIngameMenu(GuiManager guiManager) : base(guiManager, "ingame_menu")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;

            this.backButton = new GuiButtonText(new GuiText("Back to game").SetOutline(1));
            this.backButton.ButtonClickEvent += new ButtonClickEventHandler(OnBackToGameButtonClicked);
            this.backButton.Centralize();
            this.backButton.AddY(GuiUtils.GetRelativeYFromY(-backButton.GetHeight() - 5));
            this.backButton.CentralizeText();
            this.components.Add(backButton);

            this.optionsButton = new GuiButtonText(new GuiText("Options").SetOutline(1));
            this.optionsButton.ButtonClickEvent += new ButtonClickEventHandler(GuiMainMenu.OnOptionsButtonClicked);
            this.optionsButton.Centralize();
            this.optionsButton.CentralizeText();
            this.components.Add(optionsButton);

            this.mainMenuButton = new GuiButtonText(new GuiText("Exit to main menu").SetOutline(1));
            this.mainMenuButton.ButtonClickEvent += new ButtonClickEventHandler(OnExitToMainMenuButtonClicked);
            this.mainMenuButton.Centralize();
            this.mainMenuButton.AddY(GuiUtils.GetRelativeYFromY(mainMenuButton.GetHeight() + 5));
            this.mainMenuButton.CentralizeText();
            this.components.Add(mainMenuButton);

            this.quitButton = new GuiButtonText(new GuiText("Quit").SetOutline(1));
            this.quitButton.ButtonClickEvent += new ButtonClickEventHandler(GuiMainMenu.OnQuitButtonClicked);
            this.quitButton.Centralize();
            this.quitButton.SetY(mainMenuButton.GetY() + GuiUtils.GetRelativeYFromY(mainMenuButton.GetHeight() + 40));
            this.quitButton.CentralizeText();
            this.components.Add(quitButton);
        }

        private void OnBackToGameButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.ingame);
        }

        private void OnExitToMainMenuButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.mainMenu);
        }
    }
}
