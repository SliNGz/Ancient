using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.chat
{
    abstract class ChatRender
    {
        public virtual void Draw3D(ChatComponent chatComponent)
        { }

        public abstract void Draw(SpriteBatch spriteBatch, ChatComponent chatComponent, int x, int y);
    }
}
