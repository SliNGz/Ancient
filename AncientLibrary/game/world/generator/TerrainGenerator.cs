using ancient.game.entity;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.chunk;
using ancient.game.world.generator;
using ancient.game.world.generator.noise;
using ancientlib.game.entity;
using ancientlib.game.init;
using ancientlib.game.utils;
using ancientlib.game.world.biome;
using ancientlib.game.world.chunk;
using ancientlib.game.world.generator.noise;
using ancientlib.game.world.structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.generator
{
    public static class TerrainGenerator
    {
        public static SimplexNoise heightNoise;

        public static void Generate(Chunk chunk)
        {
            GenerateQuarter(chunk, 0, 0, 8, 8);
            GenerateQuarter(chunk, 8, 0, 16, 8);
            GenerateQuarter(chunk, 0, 8, 8, 16);
            GenerateQuarter(chunk, 8, 8, 16, 16);
        }

        private static void GenerateQuarter(Chunk chunk, int xMin, int zMin, int xMax, int zMax)
        {
            Biome bottomLeftBiome = BiomeManager.GetBiomeOfBlock(chunk.GetX() + xMin, chunk.GetZ() + zMin);
            Biome bottomRightBiome = BiomeManager.GetBiomeOfBlock(chunk.GetX() + xMax, chunk.GetZ() + zMin);
            Biome topLeftBiome = BiomeManager.GetBiomeOfBlock(chunk.GetX() + xMin, chunk.GetZ() + zMax);
            Biome topRightBiome = BiomeManager.GetBiomeOfBlock(chunk.GetX() + xMax, chunk.GetZ() + zMax);

            int bottomLeft = GetHeight(bottomLeftBiome, chunk.GetX() + xMin, chunk.GetZ() + zMin);
            int bottomRight = GetHeight(bottomRightBiome, chunk.GetX() + xMax, chunk.GetZ() + zMin);
            int topLeft = GetHeight(topLeftBiome, chunk.GetX() + xMin, chunk.GetZ() + zMax);
            int topRight = GetHeight(topRightBiome, chunk.GetX() + xMax, chunk.GetZ() + zMax);

            bool sameBiome = bottomLeftBiome == bottomRightBiome && bottomLeftBiome == topLeftBiome && bottomLeftBiome == topRightBiome;

            for (int x = xMin; x < xMax; ++x)
            {
                for (int z = zMin; z < zMax; ++z)
                {
                    if (x == 16)
                        continue;
                    if (z == 16)
                        continue;

                    int height = 0;

                    Biome biome = BiomeManager.GetBiomeOfBlock(chunk.GetX() + x, chunk.GetZ() + z);

                    if (sameBiome)
                        height = GetHeight(biome, chunk.GetX() + x, chunk.GetZ() + z);
                    else
                        height = (int)MathUtils.BilinearInterpolation(bottomLeft, topLeft, bottomRight, topRight,
                                                     chunk.GetX() + xMin, chunk.GetX() + xMax,
                                                     chunk.GetZ() + zMin, chunk.GetZ() + zMax,
                                                     chunk.GetX() + x, chunk.GetZ() + z);

                    biome.Generate(chunk, x, z, height);
                }
            }
        }

        private static int GetHeight(Biome biome, int x, int z)
        {
            Vector2 vector = new Vector2(x, z);

            double nx = x / (256.0);
            double nz = z / (256.0);

            double elevation = heightNoise.Evaluate(nx * biome.GetElevation(), nz * biome.GetElevation());
            double roughness = heightNoise.Evaluate(nx * biome.GetRoughness(), nz * biome.GetRoughness());
            double detail = heightNoise.Evaluate(nx * biome.GetDetail(), nz * biome.GetDetail());

            double value = 0.6 * elevation + 0.3 * roughness + 0.1 * detail;
            value = (value + 1) / 2;

            return (int)(biome.GetMaxHeight() - (biome.GetMaxHeight() - biome.GetMinHeight()) * (1 - value));
        }
    }
}
