using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketPlayerRotation : Packet
    {
        private float headYaw;
        private float headPitch;

        public PacketPlayerRotation()
        { }

        public PacketPlayerRotation(EntityPlayer player)
        {
            this.headYaw = player.GetHeadYaw();
            this.headPitch = player.GetHeadPitch();
        }

        public override void Read(BinaryReader reader)
        {
            this.headYaw = reader.ReadSingle();
            this.headPitch = reader.ReadSingle();
        }

        public override void Write(BinaryWriter writer)
        {
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
