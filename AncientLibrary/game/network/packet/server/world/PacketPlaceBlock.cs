using ancient.game.world.block;
using ancientlib.game.init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancientlib.game.utils;

namespace ancientlib.game.network.packet.server.world
{
    public class PacketPlaceBlock : Packet
    {
        private int blockID;
        private int x;
        private int y;
        private int z;

        public PacketPlaceBlock()
        { }

        public PacketPlaceBlock(Block block, int x, int y, int z)
        {
            this.blockID = Blocks.GetIDFromBlock(block);
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void Read(BinaryReader reader)
        {
            this.blockID = reader.ReadInt32();
            reader.ReadPositionInt(out x, out y, out z);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(blockID);
            writer.Write(x, y, z);
        }
        
        public int GetBlockID()
        {
            return this.blockID;
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public int GetZ()
        {
            return this.z;
        }
    }
}
