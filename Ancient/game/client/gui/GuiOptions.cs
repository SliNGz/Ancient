using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.settings;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.client.utils;

namespace ancient.game.client.gui
{
    public class GuiOptions : GuiMenuBackground
    {
        private GameSettings gameSettings;

        private GuiButtonText videoSettingsButton;
        private GuiSlider volumeSlider;

        private GuiButtonText back;

        public GuiOptions(GuiManager guiManager) : base(guiManager, "options")
        {
            this.gameSettings = Ancient.ancient.gameSettings;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.mainMenu;

            this.videoSettingsButton = new GuiButtonText(new GuiText("Video Settings").SetOutline(1));
            this.videoSettingsButton.ButtonClickEvent += new ButtonClickEventHandler(OnVideoSettingsButtonClicked);
            this.videoSettingsButton.Centralize();
            this.videoSettingsButton.CentralizeText();
            this.components.Add(videoSettingsButton);

            this.volumeSlider = new GuiSlider();
            this.volumeSlider.Centralize();
            this.volumeSlider.AddY(GuiUtils.GetRelativeYFromY(-volumeSlider.GetHeight() - 5));
            this.volumeSlider.SetValue(gameSettings.GetVolume());
            this.volumeSlider.SetGuiText(new GuiText("Volume: " + volumeSlider.GetValue() * 100).SetSize(3).SetColor(Color.White));
            this.volumeSlider.CentralizeText();
            this.volumeSlider.ValueChanged += new ValueChangedEventHandler(OnVolumeChanged);
            this.components.Add(volumeSlider);

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

            if (lastGui == guiManager.mainMenu || lastGui == guiManager.menu)
            {
                this.drawWorldBehind = lastGui.ShouldDrawWorldBehind();
                this.lastGui = lastGui;
            }
        }

        private void OnVolumeChanged(object sender, EventArgs e)
        {
            gameSettings.SetVolume(volumeSlider.GetValue());
            volumeSlider.GetGuiText().SetText("Volume: " + volumeSlider.GetValue() * 100);
        }

        private void OnVideoSettingsButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(guiManager.videoSettings);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(lastGui);
        }
    }
}
