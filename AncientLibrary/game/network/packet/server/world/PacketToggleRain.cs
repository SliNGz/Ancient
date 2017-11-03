using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.world
{
    public class PacketToggleRain : Packet
    {
        private bool isRaining;

        public PacketToggleRain()
        { }

        public PacketToggleRain(bool isRaining)
        {
            this.isRaining = isRaining;
        }

        public override void Read(BinaryReader reader)
        {
            this.isRaining = reader.ReadBoolean();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(isRaining);
        }

        public bool IsRaining()
        {
            return this.isRaining;
        }
    }
}
