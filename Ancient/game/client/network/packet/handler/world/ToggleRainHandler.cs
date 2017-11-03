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
    class ToggleRainHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketToggleRain rainPacket = (PacketToggleRain)packet;
            Ancient.ancient.world.SetRaining(rainPacket.IsRaining());
        }
    }
}
