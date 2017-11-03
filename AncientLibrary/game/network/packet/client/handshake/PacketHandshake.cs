using ancientlib.game.constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.handshake
{
    public class PacketHandshake : Packet
    {
        private double version;

        public PacketHandshake()
        {
            this.version = GameConstants.GAME_VERSION;
        }

        public override void Read(BinaryReader reader)
        {
            this.version = reader.ReadDouble();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(version);
        }

        public double GetVersion()
        {
            return this.version;
        }
    }
}
