using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancientlib.game.entity;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerTamePet : Packet
    {
        private int petID;

        public PacketPlayerTamePet()
        { }

        public PacketPlayerTamePet(EntityPet pet)
        {
            this.petID = pet.GetID();
        }

        public override void Read(BinaryReader reader)
        {
            this.petID = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(petID);
        }

        public int GetPetID()
        {
            return this.petID;
        }
    }
}
