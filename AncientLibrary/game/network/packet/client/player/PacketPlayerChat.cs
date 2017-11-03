using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketPlayerChat : Packet
    {
        private string text;

        public PacketPlayerChat()
        { }

        public PacketPlayerChat(string text)
        {
            this.text = text;
        }

        public override void Read(BinaryReader reader)
        {
            this.text = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(text);
        }

        public string GetText()
        {
            return this.text;
        }
    }
}
