using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancientlib.game.utils;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketEntityPosition : PacketEntity
    {
        private double x;
        private double y;
        private double z;

        public PacketEntityPosition()
        { }

        public PacketEntityPosition(Entity entity) : base(entity)
        {
            this.x = entity.GetX();
            this.y = entity.GetY();
            this.z = entity.GetZ();
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            reader.ReadPosition(out x, out y, out z);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public double GetX()
        {
            return this.x;
        }

        public double GetY()
        {
            return this.y;
        }

        public double GetZ()
        {
            return this.z;
        }
    }
}
