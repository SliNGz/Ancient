using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils.chat
{
    class ChatComponents
    {
        public static Dictionary<byte, Type> components = new Dictionary<byte, Type>();

        public static void Initialize()
        {
            components.Add(0, typeof(ChatComponentText));
        }

        public static byte GetIDFromChatComponent(ChatComponent chatComponent)
        {
            return components.FirstOrDefault(x => x.Value == chatComponent.GetType()).Key;
        }

        public static ChatComponent CreateChatComponentFromID(int id)
        {
            Type type = null;

            if (components.TryGetValue((byte)id, out type))
                return (ChatComponent)Activator.CreateInstance(type);

            return null;
        }
    }
}
