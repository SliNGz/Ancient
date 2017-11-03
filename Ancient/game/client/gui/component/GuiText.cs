using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.utils;
using Microsoft.Xna.Framework;
using ancient.game.client.renderer.font;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.utils;

namespace ancient.game.client.gui.component
{
    public class GuiText : GuiComponent
    {
        protected string text;

        protected float size;
        protected int spacing;

        protected int outline;
        protected Color outlineColor;

        public GuiText(string text, float size, int spacing)
        {
            this.text = text;
            this.color = Color.White;
            this.size = size;
            this.spacing = spacing;
            this.outlineColor = Color.Black;
        }

        public GuiText(string text) : this(text, 3, 1)
        {
            this.text = text;
        }

        public GuiText() : this("")
        {
            this.text = "";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            FontRenderer.DrawString(spriteBatch, text, GuiUtils.GetXFromRelativeX(this.x), GuiUtils.GetYFromRelativeY(this.y), color, size, spacing, outline, outlineColor);
        }

        public override void OnClick(MouseState mouseState)
        { }

        public override void OnHold(MouseState mouseState)
        { }

        public override void OnFocus()
        { }

        public override void OnHover()
        { }

        public override void OnRelease()
        { }

        public override void OnUnfocus()
        { }

        public override void OnUnhover()
        { }

        public string GetText()
        {
            return this.text;
        }

        public GuiText SetText(string text)
        {
            this.text = text;
            return this;
        }

        public void AddText(string add)
        {
            this.text += add;
        }

        public void AddText(char add)
        {
            this.text += add;
        }

        public new GuiText SetColor(Color color)
        {
            this.color = color;
            return this;
        }

        public float GetSize()
        {
            return this.size;
        }

        public GuiText SetSize(float size)
        {
            this.size = MathHelper.Clamp(size, 0, 64);
            return this;
        }

        public int GetSpacing()
        {
            return this.spacing;
        }

        public GuiText SetSpacing(int spacing)
        {
            this.spacing = spacing;
            return this;
        }

        public new float GetWidth()
        {
            return FontRenderer.MeasureGuiText(this);
        }

        public new float GetHeight()
        {
            return FontRenderer.fontSize * size;
        }

        public override GuiComponent Centralize()
        {
            this.x = 0.5F - GuiUtils.GetRelativeXFromX(GetWidth() / 2F);
            this.y = 0.5F - GuiUtils.GetRelativeYFromY(GetHeight() / 2F);
            return this;
        }

        public int GetOutline()
        {
            return this.outline;
        }

        public GuiText SetOutline(int outline)
        {
            this.outline = outline;
            return this;
        }

        public Color GetOutlineColor()
        {
            return this.outlineColor;
        }

        public GuiText SetOutlineColor(Color outlineColor)
        {
            this.outlineColor = outlineColor;
            return this;
        }
    }
}
