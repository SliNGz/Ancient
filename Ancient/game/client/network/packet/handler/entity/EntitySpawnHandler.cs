using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.entity;
using ancient.game.entity;
using ancientlib.game.entity;
using ancient.game.client.world;

namespace ancient.game.client.network.packet.handler.entity
{
    class EntitySpawnHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketSpawnEntity spawnEntity = (PacketSpawnEntity)packet;

            Ancient.ancient.world.SpawnEntity(spawnEntity.GetEntity());
        }
    }
}
