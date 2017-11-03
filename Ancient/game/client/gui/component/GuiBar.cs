using ancient.game.client.renderer.texture;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ancient.game.client.utils;

namespace ancient.game.client.gui.component
{
    class GuiBar : GuiComponentText
    {
        private static Texture2D barFrame = TextureManager.GetTextureFromName("bar_frame");

        public event ValueChangedEventHandler ValueChanged;

        private int value;
        private int maxValue;

        public GuiBar(string textureName) : base(textureName)
        {
            this.width = (int)(barFrame.Width * 0.925f);
            this.height = barFrame.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int x = GuiUtils.GetXFromRelativeX(this.x);
            int y = GuiUtils.GetYFromRelativeY(this.y);
            spriteBatch.Draw(barFrame, new Rectangle(x, y, width, height), Color.White);
            spriteBatch.Draw(texture, new Rectangle(x + 1, y + 1, (int)Math.Ceiling((value / (float)maxValue) * width - 2), height - 2), Color.White);

            if (guiText != null)
                guiText.Draw(spriteBatch);
        }

        public override void OnClick(MouseState mouseState)
        { }

        public override void OnHold(MouseState mouseState)
        { }

        public override void OnHover()
        { }

        public override void OnRelease()
        { }

        public override void OnUnhover()
        { }

        public int GetValue()
        {
            return this.value;
        }

        public void SetValue(int value)
        {
            this.value = MathHelper.Clamp(value, 0, maxValue);
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetMaxValue()
        {
            return this.maxValue;
        }

        public void SetMaxValue(int maxValue)
        {
            this.maxValue = maxValue;
        }
    }
}
