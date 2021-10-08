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
            x += chunk.GetX();
            y += chunk.GetY();
            z += chunk.GetZ();

            Color color = block.GetColorAt(chunk.GetWorld(), x, y, z);

      /*      int maxLight = 0;
            int sunlight = chunk.GetSunlight(x, y, z);

            Tuple<int, int, int, Block>[] neighbors = chunk.GetWorld().GetNeighborsOfBlock(x, y, z);

            for (int i = 0; i < 6; i++)
            {
                Tuple<int, int, int, Block> neighbor = neighbors[i];
                int light = chunk.GetWorld().GetBlocklight(neighbor.Item1, neighbor.Item2, neighbor.Item3);

                if (light > maxLight)
                    maxLight = light;
            }

            maxLight = Math.Max(maxLight, sunlight);

            float lerp = (float)Math.Pow(0.8, 15 - maxLight);
            Color affectedColor = Color.Lerp(Color.Black, color, lerp);
            affectedColor.A = color.A;*/

            return color;
        }
    }
}
