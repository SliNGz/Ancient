using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerChangeLevel : Packet
    {
        private int level;

        public PacketPlayerChangeLevel()
        { }

        public PacketPlayerChangeLevel(int level)
        {
            this.level = level;
        }

        public override void Read(BinaryReader reader)
        {
            this.level = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(level);
        }

        public int GetLevel()
        {
            return this.level;
        }
    }
}
