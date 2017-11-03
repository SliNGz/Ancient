using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using ancient.game.client.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui
{
    public class GuiPlayMenu : GuiMenuBackground
    {
        private GuiButtonText singleplayer;
        private GuiButtonText multiplayer;

        public GuiPlayMenu(GuiManager guiManager) : base(guiManager, "play_menu")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.mainMenu;

            this.singleplayer = new GuiButtonText(new GuiText("Singleplayer").SetOutline(1));
            this.singleplayer.ButtonClickEvent += new ButtonClickEventHandler(OnSingleplayerButtonClicked);
            this.singleplayer.Centralize();
            this.singleplayer.AddY(-0.05F);
            this.singleplayer.CentralizeText();
            this.components.Add(singleplayer);

            this.multiplayer = new GuiButtonText(new GuiText("Multiplayer").SetOutline(1));
            this.multiplayer.ButtonClickEvent += new ButtonClickEventHandler(OnMultiplayerButtonClicked);
            this.multiplayer.Centralize();
            this.multiplayer.AddY(0.05F);
            this.multiplayer.CentralizeText();
            this.components.Add(multiplayer);
        }

        private void OnSingleplayerButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.worldCreation);
        }

        private void OnMultiplayerButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.serverBrowser);
        }
    }
}
