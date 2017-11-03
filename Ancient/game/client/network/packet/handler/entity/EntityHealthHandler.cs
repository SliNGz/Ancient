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
using ancientlib.game.utils;

namespace ancient.game.client.network.packet.handler.entity
{
    class EntityHealthHandler : AbstractEntityHandler
    {
        public override void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection)
        {
            PacketEntityHealth healthPacket = (PacketEntityHealth)packet;

            EntityLiving living = (EntityLiving)entity;

            living.SetHealth(healthPacket.GetHealth());
        }
    }
}
