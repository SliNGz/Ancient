using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.entity
{
    public abstract class PacketEntity : Packet
    {
        protected int id;

        public PacketEntity()
        { }

        public PacketEntity(Entity entity)
        {
            this.id = entity.GetID();
        }

        public override void Read(BinaryReader reader)
        {
            this.id = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(id);
        }

        public int GetID()
        {
            return this.id;
        }
    }
}
