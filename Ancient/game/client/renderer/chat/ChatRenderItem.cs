using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ancient.game.client.renderer.item;
using ancientlib.game.item;

namespace ancient.game.client.renderer.chat
{
    class ChatRenderItem : ChatRender
    {
        public static RenderTarget2D renderTarget;

        public static void Initialize()
        {
            renderTarget = new RenderTarget2D(Ancient.ancient.GraphicsDevice, Ancient.ancient.device.Viewport.Width, Ancient.ancient.device.Viewport.Height);
        }

        public override void Draw3D(ChatComponent chatComponent)
        {
            ChatComponentItem itemComponent = (ChatComponentItem)chatComponent;
            Item item = itemComponent.GetItem();
            ItemRenderer.DrawToRenderTarget(renderTarget, item, itemComponent.GetYaw(), 0, 0, item.GetHandScale().X * 0.25F, item.GetHandScale().Y * 0.25F, item.GetHandScale().Z * 0.25F);
        }

        public override void Draw(SpriteBatch spriteBatch, ChatComponent chatComponent, int x, int y)
        {
            spriteBatch.Draw(renderTarget, new Vector2(-Ancient.ancient.width / 2F + x, -Ancient.ancient.height / 2F + y), chatComponent.GetColor());
        }
    }
}
