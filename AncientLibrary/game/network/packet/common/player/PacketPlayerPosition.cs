using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancientlib.game.utils;

namespace ancientlib.game.network.packet.common.player
{
    public class PacketPlayerPosition : Packet
    {
        private float x;
        private float y;
        private float z;

        public PacketPlayerPosition()
        { }

        public PacketPlayerPosition(EntityPlayer player)
        {
            this.x = (float)player.GetX();
            this.y = (float)player.GetY();
            this.z = (float)player.GetZ();
        }

        public override void Read(BinaryReader reader)
        {
            this.x = reader.ReadSingle();
            this.y = reader.ReadSingle();
            this.z = reader.ReadSingle();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public float GetX()
        {
            return this.x;
        }

        public float GetY()
        {
            return this.y;
        }

        public float GetZ()
        {
            return this.z;
        }
    }
}
