using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketPlayerUseItem : Packet
    {
        private UseAction useAction;

        public PacketPlayerUseItem()
        { }

        public PacketPlayerUseItem(UseAction useAction)
        {
            this.useAction = useAction;
        }

        public override void Read(BinaryReader reader)
        {
            this.useAction = (UseAction)reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)useAction);
        }

        public UseAction GetUseAction()
        {
            return this.useAction;
        }
    }

    public enum UseAction
    {
        PRESS_LEFT = 0x00,
        RELEASE_LEFT = 0x01,
        HOLD_LEFT = 0x02,
        PRESS_RIGHT = 0x03,
        RELEASE_RIGHT = 0x04,
        HOLD_RIGHT = 0x05,
    }
}
