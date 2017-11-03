using ancient.game.world.block.type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using Microsoft.Xna.Framework;
using ancientlib.game.block;
using ancientlib.game.init;

namespace ancient.game.world.block
{
    public class BlockWater : Block
    {
        public BlockWater() : base("Water", BlockType.water)
        {
            this.dimensions = new Vector3(1, 0.8F, 1);
            this.speedModifier = 0.65F;
            this.color = new Color(0, 85, 255, 128);
            this.secondaryColor = color;
        }

        public override bool IsFullBlock()
        {
            return false;
        }

        public override bool ShouldRenderFace(Block neighbor, int xOffset, int yOffset, int zOffset)
        {
            if (neighbor is BlockWater)
                return false;

            if (yOffset == 1)
            {
                if (neighbor is BlockIce)
                    return false;

                return true;
            }

            return base.ShouldRenderFace(neighbor, xOffset, yOffset, zOffset);
        }
    }
}
