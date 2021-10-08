using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancientlib.game.network;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.entity;

namespace ancient.game.client.network.packet.handler.entity
{
    class EntityHeadRotationHandler : AbstractEntityHandler
    {
        public override void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection)
        {
            PacketEntityHeadRotation headRotPacket = (PacketEntityHeadRotation)packet;
            EntityLiving living = (EntityLiving)entity;

            living.SetHeadYaw(headRotPacket.GetHeadYaw());
            living.SetHeadPitch(headRotPacket.GetHeadPitch());
        }
    }
}
