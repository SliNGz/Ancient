using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancient.game.entity.player;
using ancientlib.game.utils;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketCharacterCreationStatus : Packet
    {
        private CharacterStatus status;

        public PacketCharacterCreationStatus()
        { }

        public PacketCharacterCreationStatus(CharacterStatus status)
        {
            this.status = status;
        }

        public override void Read(BinaryReader reader)
        {
            this.status = (CharacterStatus)reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)status);
        }

        public CharacterStatus GetStatus()
        {
            return this.status;
        }
    }

    public enum CharacterStatus
    {
        REQUEST_CHARACTER_CREATION = 0x00,
        SPAWN_CHARACTER = 0x01,
    }
}
