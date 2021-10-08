using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.chat
{
    class ChatRenderer
    {
        public static void Draw3D(ChatComponent chatComponent)
        {
            ChatRenderers.GetRenderFromChatComponent(chatComponent).Draw3D(chatComponent);
        }

        public static void Draw(SpriteBatch spriteBatch, ChatComponent chatComponent, int x, int y)
        {
            ChatRenderers.GetRenderFromChatComponent(chatComponent).Draw(spriteBatch, chatComponent, x, y);
        }
    }
}
