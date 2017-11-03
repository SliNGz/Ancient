using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.utils;
using Microsoft.Xna.Framework;
using ancient.game.client.renderer.texture;

namespace ancient.game.client.gui.component
{
    public class GuiComponentText : GuiComponent
    {
        protected GuiText guiText;

        public GuiComponentText(string textureName) : base()
        {
            if (textureName != "null")
            {
                this.texture = TextureManager.GetTextureFromName(textureName);
                this.width = texture.Width;
                this.height = texture.Height;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, new Rectangle(GuiUtils.GetXFromRelativeX(this.x), GuiUtils.GetYFromRelativeY(this.y), width, height), color);

            if (guiText != null)
                guiText.Draw(spriteBatch);
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

        public GuiText GetGuiText()
        {
            return this.guiText;
        }

        public GuiComponentText SetGuiText(GuiText guiText)
        {
            this.guiText = guiText;
            return this;
        }

        public void CentralizeText()
        {
            guiText.SetX(this.x + GuiUtils.GetRelativeXFromX(this.width / 2) - GuiUtils.GetRelativeXFromX(guiText.GetWidth() / 2));
            guiText.SetY(this.y + GuiUtils.GetRelativeYFromY(this.height / 2 - guiText.GetHeight() / 2));
        }
    }
}
