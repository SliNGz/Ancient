using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketPlayerChangeSlot : Packet
    {
        private byte slot;

        public PacketPlayerChangeSlot()
        { }

        public PacketPlayerChangeSlot(byte slot)
        {
            this.slot = slot;
        }

        public override void Read(BinaryReader reader)
        {
            this.slot = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(slot);
        }

        public int GetSlot()
        {
            return this.slot;
        }
    }
}
