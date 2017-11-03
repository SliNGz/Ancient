using ancient.game.client.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui.component.button
{
    class GuiButtonText : GuiButton
    {
        private Color holdColor;
        private Color hoverColor;

        public GuiButtonText(GuiText guiText) : base("null")
        {
            this.guiText = guiText;
            MatchDimensionsToText();
            this.holdColor = Color.Gray;
            this.hoverColor = Color.LightGray;
        }

        public override void OnHold(MouseState mouseState)
        {
            this.guiText.SetColor(this.holdColor);
        }

        public override void OnRelease()
        {
            this.guiText.SetColor(this.color);
        }

        public override void OnHover()
        {
            this.guiText.SetColor(this.hoverColor);
        }

        public override void OnUnhover()
        {
            this.guiText.SetColor(this.color);
        }

        public Color GetHoldColor()
        {
            return this.holdColor;
        }

        public void SetHoldColor(Color holdColor)
        {
            this.holdColor = holdColor;
        }

        public Color GetHoverColor()
        {
            return this.hoverColor;
        }

        public void SetHoverColor(Color hoverColor)
        {
            this.hoverColor = hoverColor;
        }

        public void MatchDimensionsToText()
        {
            this.width = (int)Math.Round(guiText.GetWidth());
            this.height = (int)Math.Round(guiText.GetHeight());
        }
    }
}
