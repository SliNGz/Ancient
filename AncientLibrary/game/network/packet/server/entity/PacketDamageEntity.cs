using ancientlib.game.entity;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketDamageEntity : PacketEntity
    {
        private AttackInfo attackInfo;

        public PacketDamageEntity()
        { }

        public PacketDamageEntity(EntityLiving living, AttackInfo attackInfo)
        {
            this.id = living.GetID();
            this.attackInfo = attackInfo;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.attackInfo = new AttackInfo();
            this.attackInfo.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            this.attackInfo.Write(writer);
        }

        public AttackInfo GetAttackInfo()
        {
            return this.attackInfo;
        }
    }
}
