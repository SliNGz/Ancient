using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.player;

namespace ancient.game.client.network.packet.handler.player
{
    class PlayerChangeClassHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerChangeClass classPacket = (PacketPlayerChangeClass)packet;

            Ancient.ancient.player.SetClass(classPacket.GetClass());
        }
    }
}
