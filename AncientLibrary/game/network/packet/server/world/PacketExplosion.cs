using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.world
{
    public class PacketExplosion : Packet
    {
        private int x;
        private int y;
        private int z;
        private float a;
        private float b;
        private float c;

        public PacketExplosion()
        { }

        public PacketExplosion(int x, int y, int z, float a, float b, float c)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadPositionInt(out x, out y, out z);
            this.a = reader.ReadSingle();
            this.b = reader.ReadSingle();
            this.c = reader.ReadSingle();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(x, y, z);
            writer.Write(a);
            writer.Write(b);
            writer.Write(c);
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

        public float GetA()
        {
            return this.a;
        }

        public float GetB()
        {
            return this.b;
        }

        public float GetC()
        {
            return this.c;
        }
    }
}
