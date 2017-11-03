using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.utils;
using ancient.game.entity;
using ancientlib.game.entity;

namespace ancient.game.client.network.packet.handler.entity
{
    class EntityDamagedHandler : AbstractEntityHandler
    {
        public override void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection)
        {
            PacketDamageEntity damagePacket = (PacketDamageEntity)packet;

            AttackInfo attackInfo = damagePacket.GetAttackInfo();
            ((EntityLiving)entity).Damage(attackInfo);
        }
    }
}
