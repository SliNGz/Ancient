using ancient.game.entity.player;
using ancientlib.game.utils.chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.common.chat
{
    public class PacketChatComponent : Packet
    {
        private ChatComponent chatComponent;

        public PacketChatComponent()
        { }

        public PacketChatComponent(ChatComponent chatComponent)
        {
            this.chatComponent = chatComponent;
        }

        public override void Read(BinaryReader reader)
        {
            this.chatComponent = ChatComponents.CreateChatComponentFromID(reader.ReadByte());
            chatComponent.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(ChatComponents.GetIDFromChatComponent(chatComponent));
            chatComponent.Write(writer);
        }

        public ChatComponent GetChatComponent()
        {
            return this.chatComponent;
        }
    }
}
