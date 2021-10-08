using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block.type
{
    public class BlockType
    {
        private string name;
        private bool isSolid;

        public static readonly BlockType air = new BlockType("Air").SetIsSolid(false);
        public static readonly BlockType dirt = new BlockType("Dirt");
        public static readonly BlockType grass = new BlockType("Grass");
        public static readonly BlockType wood = new BlockType("Wood");
        public static readonly BlockType leaves = new BlockType("Leaves");
        public static readonly BlockType water = new BlockType("Water").SetIsSolid(false);
        public static readonly BlockType sand = new BlockType("Sand");
        public static readonly BlockType snow = new BlockType("Snow");
        public static readonly BlockType ice = new BlockType("Ice");
        public static readonly BlockType cloud = new BlockType("Cloud");
        public static readonly BlockType stone = new BlockType("Stone");

        public BlockType(string name)
        {
            this.name = name;
            this.isSolid = true;
        }

        public BlockType SetName(string name)
        {
            this.name = name;
            return this;
        }

        public string GetName()
        {
            return this.name;
        }

        public BlockType SetIsSolid(bool isSolid)
        {
            this.isSolid = isSolid;
            return this;
        }

        public bool IsSolid()
        {
            return this.isSolid;
        }
    }
}