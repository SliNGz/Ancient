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
        private VideoSettings videoSettings;

        private GuiSlider renderDistanceSlider;
        private GuiSlider antiAliasingSlider;

        private GuiButtonText back;

        public GuiVideoSettings(GuiManager guiManager) : base(guiManager, "video_settings")
        {
            this.videoSettings = Ancient.ancient.gameSettings.GetVideoSettings();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.options;

            this.renderDistanceSlider = new GuiSlider();
            this.renderDistanceSlider.SetX(0.2F);
            this.renderDistanceSlider.SetY(0.2F);
            this.renderDistanceSlider.SetGuiText(new GuiText("Render Distance: " + videoSettings.GetRenderDistance(), 3, 1).SetOutline(1));
            this.renderDistanceSlider.CentralizeText();
            this.renderDistanceSlider.ValueChanged += new ValueChangedEventHandler(OnRenderDistanceValueChanged);
            this.components.Add(renderDistanceSlider);

            this.antiAliasingSlider = new GuiSlider();
            this.antiAliasingSlider.SetX(0.6F);
            this.antiAliasingSlider.SetY(0.2F);
            this.antiAliasingSlider.SetGuiText(new GuiText("Anti Aliasing: " + videoSettings.GetAntiAliasing(), 3, 1).SetOutline(1));
            this.antiAliasingSlider.CentralizeText();
            this.antiAliasingSlider.ValueChanged += new ValueChangedEventHandler(OnAntiAliasingValueChanged);
            this.components.Add(antiAliasingSlider);

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

        public override void OnClose()
        {
            base.OnClose();
            videoSettings.SaveSettings();
        }

        private void OnRenderDistanceValueChanged(object sender, EventArgs e)
        {
            int renderDistance = GetRenderDistanceFromValue();
            this.renderDistanceSlider.GetGuiText().SetText("Render Distance: " + renderDistance);
            videoSettings.SetRenderDistance(renderDistance);
        }

        private int GetRenderDistanceFromValue()
        {
            return (int)(VideoSettings.MIN_RENDER_DISTANCE + renderDistanceSlider.GetValue() * (VideoSettings.MAX_RENDER_DISTANCE - VideoSettings.MIN_RENDER_DISTANCE));
        }

        private void OnAntiAliasingValueChanged(object sender, EventArgs e)
        {
            int antiAliasing = GetAntiAliasingFromValue();
            this.antiAliasingSlider.GetGuiText().SetText("Anti Aliasing: " + antiAliasing);
            videoSettings.SetAntiAliasing(antiAliasing);
        }

        private int GetAntiAliasingFromValue()
        {
            return (int)(antiAliasingSlider.GetValue() * VideoSettings.MAX_ANTI_ALIASING);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            guiManager.DisplayGui(lastGui);
        }
    }
}
