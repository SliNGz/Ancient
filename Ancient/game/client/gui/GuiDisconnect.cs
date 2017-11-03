using ancient.game.client.gui.component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.gui.component.button;
using ancient.game.client.utils;
using ancientlib.game.utils;

namespace ancient.game.client.gui
{
    class GuiDisconnect : GuiMenuBackground
    {
        private GuiText dcText;
        private GuiButton okButton;

        public GuiDisconnect(GuiManager guiManager, string dcMessage) : base(guiManager, null)
        {
            this.drawWorldBehind = false;
            this.lastGui = guiManager.serverBrowser;

            dcText = new GuiText(dcMessage).SetOutline(1);
            dcText.Centralize();
            this.components.Add(dcText);

            okButton = new GuiButton();
            okButton.ButtonClickEvent += new ButtonClickEventHandler(OnOkButtonClicked);
            okButton.Centralize();
            okButton.AddY(GuiUtils.GetRelativeYFromY(dcText.GetHeight() + 80));
            okButton.SetGuiText(new GuiText("Ok"));
            okButton.CentralizeText();
            this.components.Add(okButton);
        }

        private void OnOkButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.mainMenu);
        }
    }
}
