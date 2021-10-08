using ancientlib.game.utils.chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.chat
{
    class ChatRenderers
    {
        public static Dictionary<Type, ChatRender> renderers = new Dictionary<Type, ChatRender>();

        public static void Initialize()
        {
            renderers.Add(typeof(ChatComponentText), new ChatRenderText());
            renderers.Add(typeof(ChatComponentItem), new ChatRenderItem());
        }

        public static ChatRender GetRenderFromChatComponent(ChatComponent chatComponent)
        {
            ChatRender chatRender = null;
            renderers.TryGetValue(chatComponent.GetType(), out chatRender);

            return chatRender;
        }
    }
}
