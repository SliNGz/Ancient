using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketEntityRotation : PacketEntity
    {
        private float yaw;

        public PacketEntityRotation()
        { }

        public PacketEntityRotation(Entity entity) : base(entity)
        {
            this.yaw = entity.GetYaw();
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.yaw = reader.ReadSingle();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(yaw);
        }

        public float GetYaw()
        {
            return this.yaw;
        }
    }
}
