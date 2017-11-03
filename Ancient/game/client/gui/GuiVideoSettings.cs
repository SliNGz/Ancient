using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using ancient.game.client.settings;
using ancient.game.client.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ancient.game.client.gui
{
    public class GuiVideoSettings : GuiMenuBackground
    {
        private GameSettings gameSettings;

        private GuiButtonText back;

        public GuiVideoSettings(GuiManager guiManager) : base(guiManager, "video_settings")
        {
            this.gameSettings = Ancient.ancient.gameSettings;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.options;

            this.back = new GuiButtonText(new GuiText("Back").SetOutline(1));
            this.back.ButtonClickEvent += new ButtonClickEventHandler(OnBackButtonClicked);
            this.back.Centralize();
            this.back.AddY(GuiUtils.GetRelativeYFromY(back.GetHeight() + 40));
            this.back.CentralizeText();
            this.components.Add(back);
        }

        public override void OnDisplay(Gui lastGui)
        {
            base.OnDisplay(lastGui);

            if (lastGui == guiManager.options)
                this.drawWorldBehind = lastGui.ShouldDrawWorldBehind();
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(lastGui);
        }
    }
}
