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

namespace ancient.game.client.network.packet.handler.entity
{
    class EntityRotationHandler : AbstractEntityHandler
    {
        public override void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection)
        {
            PacketEntityRotation rotPacket = (PacketEntityRotation)packet;

            entity.SetYaw(rotPacket.GetYaw());

            if (entity is EntityLiving)
                ((EntityLiving)entity).SetHeadYaw(rotPacket.GetYaw());
        }
    }
}
