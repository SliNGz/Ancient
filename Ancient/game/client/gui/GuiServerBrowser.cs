using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using ancient.game.client.network;
using ancient.game.client.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui
{
    public class GuiServerBrowser : GuiMenuBackground
    {
        private GuiTextBox ipTextBox;
        private GuiButton connectButton;

        public GuiServerBrowser(GuiManager guiManager) : base(guiManager, "server_browser")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.playMenu;

            this.ipTextBox = new GuiTextBox();
            this.ipTextBox.GetTextBox().SetText("192.168.1.18").Centralize();
            this.ipTextBox.SetWidth(440);
            this.ipTextBox.Centralize();
            this.ipTextBox.SetGuiText(new GuiText("IP Address:", 3, 2).SetOutline(1));
            this.ipTextBox.CentralizeText();
            this.ipTextBox.GetGuiText().SetY(this.ipTextBox.GetY() + GuiUtils.GetRelativeYFromY(-this.ipTextBox.GetGuiText().GetHeight()));
            this.ipTextBox.GetTextBox().SetSize(3).SetOutline(1);
            this.components.Add(ipTextBox);

            this.connectButton = new GuiButton();
            this.connectButton.ButtonClickEvent += new ButtonClickEventHandler(OnConnectButtonClicked);
            this.connectButton.Centralize();
            this.connectButton.AddY(GuiUtils.GetRelativeYFromY(this.ipTextBox.GetHeight() + 10));
            this.connectButton.SetGuiText(new GuiText("Connect"));
            this.connectButton.CentralizeText();
            this.components.Add(connectButton);
        }

        private void OnConnectButtonClicked(object sender, EventArgs e)
        {
            string ipAddress = ipTextBox.GetTextBox().GetText();
            Console.WriteLine("Connected: " + NetworkUtils.TryConnect(ipAddress));
        }
    }
}
