using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketSelectCharacter : Packet
    {
        private int characterIndex;

        public PacketSelectCharacter()
        { }

        public PacketSelectCharacter(int characterIndex)
        {
            this.characterIndex = characterIndex;
        }

        public override void Read(BinaryReader reader)
        {
            this.characterIndex = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(characterIndex);
        }

        public int GetCharacterIndex()
        {
            return this.characterIndex;
        }
    }
}
