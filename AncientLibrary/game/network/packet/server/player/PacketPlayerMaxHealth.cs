using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerMaxHealth : Packet
    {
        private int maxHealth;

        public PacketPlayerMaxHealth()
        { }

        public PacketPlayerMaxHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
        }

        public override void Read(BinaryReader reader)
        {
            this.maxHealth = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(maxHealth);
        }

        public int GetMaxHealth()
        {
            return this.maxHealth;
        }
    }
}
