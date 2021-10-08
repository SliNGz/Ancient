using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerChangeExp : Packet
    {
        private int exp;

        public PacketPlayerChangeExp()
        { }

        public PacketPlayerChangeExp(int exp)
        {
            this.exp = exp;
        }

        public override void Read(BinaryReader reader)
        {
            this.exp = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(exp);
        }

        public int GetExp()
        {
            return this.exp;
        }
    }
}
