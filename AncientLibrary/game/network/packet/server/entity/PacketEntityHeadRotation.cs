using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancientlib.game.entity;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketEntityHeadRotation : PacketEntity
    {
        private float headYaw;
        private float headPitch;

        public PacketEntityHeadRotation()
        { }

        public PacketEntityHeadRotation(EntityLiving living) : base(living)
        {
            this.headYaw = living.GetHeadYaw();
            this.headPitch = living.GetHeadPitch();
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.headYaw = reader.ReadSingle();
            this.headPitch = reader.ReadSingle();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(headYaw);
            writer.Write(headPitch);
        }

        public float GetHeadYaw()
        {
            return this.headYaw;
        }

        public float GetHeadPitch()
        {
            return this.headPitch;
        }
    }
}
