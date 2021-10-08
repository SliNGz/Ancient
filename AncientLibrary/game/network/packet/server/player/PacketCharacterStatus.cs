using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancient.game.entity.player;
using ancientlib.game.utils;
using ancientlib.game.network.packet.client.player;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketCharacterStatus : Packet
    {
        private CharacterStatus status;

        public PacketCharacterStatus()
        { }

        public PacketCharacterStatus(CharacterStatus status)
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
        REQUEST_CHARACTER_SELECTION = 0x00,
        CREATE_CHARACTER = 0x01,
        SELECT_CHARACTER = 0x02,
        ERROR_NAME_TAKEN = 0x03,
        ERROR_REACHED_MAX_CHARACTERS = 0x04
    }
}
