using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ancient.game.client.utils;
using ancient.game.client.renderer.texture;

namespace ancient.game.client.gui.component
{
    class GuiTexture : GuiComponent
    {
        public GuiTexture(string textureName)
        {
            this.texture = TextureManager.GetTextureFromName(textureName);
            this.width = texture.Width;
            this.height = texture.Height;
        }

        public GuiTexture(Texture2D texture)
        {
            this.texture = texture;
            this.width = texture.Width;
            this.height = texture.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(GuiUtils.GetXFromRelativeX(this.x), GuiUtils.GetYFromRelativeY(this.y), width, height), Color.White);
        }

        public override void OnClick(MouseState mouseState)
        { }

        public override void OnFocus()
        { }

        public override void OnHold(MouseState mouseState)
        { }

        public override void OnHover()
        { }

        public override void OnRelease()
        { }

        public override void OnUnfocus()
        { }

        public override void OnUnhover()
        { }
    }
}
