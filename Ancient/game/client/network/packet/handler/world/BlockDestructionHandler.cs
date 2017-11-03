using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.world;

namespace ancient.game.client.network.packet.handler.world
{
    class BlockDestructionHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketDestroyBlock destroyPacket = (PacketDestroyBlock)packet;

            Ancient.ancient.world.DestroyBlock(destroyPacket.GetX(), destroyPacket.GetY(), destroyPacket.GetZ());
        }
    }
}
