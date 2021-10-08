using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.chat;
using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.network.packet.handler.chat
{
    class ChatComponentClientHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketChatComponent chatPacket = (PacketChatComponent)packet;
            Ancient.ancient.world.AddChatComponent(chatPacket.GetChatComponent());
        }
    }
}
