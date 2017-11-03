using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.world;
using ancientlib.game.init;

namespace ancient.game.client.network.packet.handler.world
{
    class BlockPlacementHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlaceBlock placePacket = (PacketPlaceBlock)packet;

            Ancient.ancient.world.SetBlock(Blocks.GetBlockFromID(placePacket.GetBlockID()), placePacket.GetX(), placePacket.GetY(), placePacket.GetZ());
        }
    }
}
