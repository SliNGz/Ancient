using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.common.status
{
    public class PacketDisconnect : Packet
    {
        private string message;

        public PacketDisconnect()
        {
            this.message = "";
        }

        public PacketDisconnect(string message)
        {
            this.message = message;
        }

        public override void Read(BinaryReader reader)
        {
            this.message = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(message);
        }

        public string GetMessage()
        {
            return this.message;
        }
    }
}
