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
    class ExplosionHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketExplosion explodePacket = (PacketExplosion)packet;

            Ancient.ancient.world.CreateExplosion(explodePacket.GetX(), explodePacket.GetY(), explodePacket.GetZ(), explodePacket.GetA(), explodePacket.GetB(), explodePacket.GetC());
        }
    }
}
