using ancientlib.game.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketEntityHealth : PacketEntity
    {
        private int health;

        public PacketEntityHealth()
        { }

        public PacketEntityHealth(EntityLiving living) : base(living)
        {
            this.health = living.GetHealth();
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.health = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(health);
        }

        public int GetHealth()
        {
            return this.health;
        }
    }
}
