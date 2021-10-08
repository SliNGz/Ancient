using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.client.renderer.font;
using Microsoft.Xna.Framework;

namespace ancient.game.client.renderer.chat
{
    class ChatRenderText : ChatRender
    {
        public override void Draw(SpriteBatch spriteBatch, ChatComponent chatComponent, int x, int y)
        {
            ChatComponentText textComponent = (ChatComponentText)chatComponent;
            FontRenderer.DrawString(spriteBatch, textComponent.GetText(), x, y, textComponent.GetColor(), 3, 1, 0, Color.Black);
        }
    }
}
