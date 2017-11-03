using ancient.game.world;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.chunk;
using ancientlib.game.init;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.utils
{
    public class BlockColorizer
    {
        public static Color GetColorOfBlock(Block block, Chunk chunk, int x, int y, int z)
        {
            Color color = GetBlockNoiseColor(block, chunk, x, y, z);
            color = Utils.GetColorAffectedByLight(chunk.GetWorld(), color, chunk.GetX() + x, chunk.GetY() + y, chunk.GetZ() + z);
            return color;
        }

        public static Color GetBlockNoiseColor(Block block, Chunk chunk, int x, int y, int z)
        {
            x += chunk.GetX();
            y += chunk.GetY();
            z += chunk.GetZ();

            float noise = (float)(chunk.GetWorld().GetSimplexNoise().Evaluate(x / 32.0, y / 2.0, z / 32.0));
            noise = (noise + 1) / 2;

            return Color.Lerp(block.GetColor(), block.GetSecondaryColor(), noise);
        }
    }
}
