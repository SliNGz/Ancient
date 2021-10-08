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
using ancientlib.game.world.biome;

namespace ancient.game.world.block
{
    public class BlockWater : Block
    {
        public BlockWater() : base("Water", BlockType.water)
        {
            this.dimensions = new Vector3(1, 0.8F, 1);
            this.speedModifier = 0.65F;
            this.color = new Color(148, 232, 255, 192);
            this.secondaryColor = color;// new Color(59, 216, 128);
        }

        public override Vector3 GetRenderDimensions(World world, int x, int y, int z)
        {
            Vector3 dimensions = this.dimensions;
            Block topBlock = world.GetBlock(x, y + 1, z);

            if (topBlock != null && topBlock is BlockWater)
                dimensions.Y = 1;

            return dimensions;
        }

        public override bool IsFullBlock()
        {
            return false;
        }

        public override bool ShouldRenderFace(World world, int x, int y, int z, Block neighbor, int xOffset, int yOffset, int zOffset)
        {
            if (neighbor is BlockWater)
                return false;

            if (yOffset == 1)
            {
                if (neighbor is BlockIce)
                    return false;

                return true;
            }

            return base.ShouldRenderFace(world, x, y, z, neighbor, xOffset, yOffset, zOffset);
        }

        public override Color GetColorAt(World world, int x, int y, int z)
        {
            Biome biome = world.GetBiomeAt(x, z);
            return biome.GetWaterColor();
        }

        public override Vector4 GetShaderTechnique(World world, int x, int y, int z)
        {
            Biome biome = world.GetBiomeAt(x, z);

            return new Vector4(0, 0, 0, biome is BiomeOcean ? 1 : 0);
        }
    }
}
