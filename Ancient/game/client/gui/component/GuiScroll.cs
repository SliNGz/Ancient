using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ancient.game.client.utils;
using ancient.game.input;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.client.renderer.texture;

namespace ancient.game.client.gui.component
{
    class GuiScroll : GuiSlider
    {
        public static Texture2D scroll = TextureManager.GetTextureFromName("scroll");
        public static Texture2D scroll_arrow = TextureManager.GetTextureFromName("scroll_arrow");

        private float scrollY;
        private int scrollHeight;

        public GuiScroll()
        {
            this.value = 0;
            this.texture = scroll;
            this.width = texture.Width;
            this.height = texture.Height;

            this.scrollHeight = 134;
        }

        public override void OnClick(MouseState mouseState)
        {
            base.OnClick(mouseState);
            int y = mouseState.Y - GuiUtils.GetYFromRelativeY(scrollY);
            SetValue(MathHelper.Clamp(y / (float)(scrollHeight), 0, 1));
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            if (InputHandler.IsScrollingUp())
                SetValue(value - 0.05F);
            else if (InputHandler.IsScrollingDown())
                SetValue(value + 0.05F);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(scroll_arrow, new Rectangle(GuiUtils.GetXFromRelativeX(x), GuiUtils.GetYFromRelativeY(scrollY) - width, width, width), Color.White);
            spriteBatch.Draw(scroll_arrow, new Rectangle(GuiUtils.GetXFromRelativeX(x), GuiUtils.GetYFromRelativeY(scrollY) + scrollHeight, width, width), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
        }

        public override GuiComponent SetY(float y)
        {
            y += GuiUtils.GetRelativeYFromY(width);
            this.scrollY = y;
            return base.SetY(y);
        }

        public override void SetValue(float value)
        {
            base.SetValue(value);
            this.y = scrollY + GuiUtils.GetRelativeYFromY(MathHelper.Clamp((scrollHeight - height) * this.value, 0, scrollHeight - height));
        }

        public GuiScroll SetScrollHeight(int scrollHeight)
        {
            this.scrollHeight = scrollHeight;
            return this;
        }

        public override void ScaleToMatchResolution()
        {
            base.ScaleToMatchResolution();
            this.scrollHeight = GuiUtils.GetScaledY(scrollHeight);
        }
    }
}
