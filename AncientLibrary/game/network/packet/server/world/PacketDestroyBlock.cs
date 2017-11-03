using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.world
{
    public class PacketDestroyBlock : Packet
    {
        private int x;
        private int y;
        private int z;

        public PacketDestroyBlock()
        { }

        public PacketDestroyBlock(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadPositionInt(out x, out y, out z);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(x, y, z);
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public int GetZ()
        {
            return this.z;
        }
    }
}
