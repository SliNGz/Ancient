using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.renderer.texture;

namespace ancient.game.client.gui.component
{
    class GuiCursor : GuiComponent
    {
        public GuiCursor()
        {
            this.texture = TextureManager.GetTextureFromName("cursor");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
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

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            this.x = mouseState.X;
            this.y = mouseState.Y;
        }
    }
}
