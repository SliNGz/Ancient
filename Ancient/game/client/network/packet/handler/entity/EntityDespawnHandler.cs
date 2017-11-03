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

namespace ancient.game.client.network.packet.handler.entity
{
    class EntityDespawnHandler : AbstractEntityHandler
    {
        public override void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection)
        {
            Ancient.ancient.world.DespawnEntity(entity);
        }
    }
}
