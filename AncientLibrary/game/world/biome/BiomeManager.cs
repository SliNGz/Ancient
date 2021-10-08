using ancient.game.world.generator.noise;
using ancientlib.game.init;
using ancientlib.game.world.structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.biome
{
    public class BiomeManager
    {
        private static int BIOME_SPREAD = 1024;
        private static SimplexNoise temperatureNoise = new SimplexNoise(15050);
        private static SimplexNoise rainfallNoise = new SimplexNoise(150502000);

        public static List<Biome> biomes = new List<Biome>();

        public static readonly Biome taiga = new BiomeTaiga(false).SetTemperatureRainfall(0.375F, 0.2F);
        public static readonly Biome taiga_hills = (new BiomeTaiga(false).SetTemperatureRainfall(0.375F, 0.35F).SetHeightMapModifiers(4, 1, 16)).SetName("taiga_hills");
        public static readonly Biome taiga_snow = new BiomeTaiga(true).SetTreeStructure("tree_snow", Structures.TREE_SNOW_NUM).SetTemperatureRainfall(0.25F, 0.2F).SetHeight(128, 64)
            .SetTopBlock(Blocks.snow).SetBGMList(new List<string>() { "bgm_snow_0" });

        public static readonly Biome tundra = new BiomeTundra().SetTemperatureRainfall(0, 0);
        public static readonly Biome desert = new BiomeDesert().SetTemperatureRainfall(0.7F, 0);
        public static readonly Biome ocean = new BiomeOcean().SetTemperatureRainfall(0.5F, 0.5F);

        public static readonly Biome skyland = new BiomeSkyland().SetTemperatureRainfall(0.6F, 0.5F);

        public static readonly Biome jungle = new BiomeJungle().SetTemperatureRainfall(1, 1);

        public static readonly Biome blossom = new BiomeBlossom("blossom").SetTemperatureRainfall(0.25F, 0.4F);

        public static readonly Biome flat = new BiomeSkyland().SetHeightMapModifiers(0, 0, 0);

        public static void Initialize()
        {
            AddBiome(taiga);
            AddBiome(taiga_hills);
            AddBiome(taiga_snow);
            AddBiome(tundra);
            AddBiome(desert);
            AddBiome(ocean);
            AddBiome(skyland);
            AddBiome(jungle);
            AddBiome(blossom);
        }

        private static void AddBiome(Biome biome)
        {
            biomes.Add(biome);
        }

        public static float GetTemperature(int x, int z)
        {
            return (float)(temperatureNoise.Evaluate(x / (float)BIOME_SPREAD, z / (float)BIOME_SPREAD) + 1) / 2F;
        }

        public static float GetRainfall(int x, int z)
        {
            return (float)(rainfallNoise.Evaluate(x / (float)BIOME_SPREAD, z / (float)BIOME_SPREAD) + 1) / 2F;
        }

        public static Biome GetBiomeAt(int x, int z)
        {
            float temperature = GetTemperature(x, z);
            float rainfall = GetRainfall(x, z);

            Biome biome = null;
            Vector2 point = new Vector2(temperature, rainfall);
            float minDistance = int.MaxValue;

            for (int i = 0; i < biomes.Count; i++)
            {
                Biome currentBiome = biomes[i];
                Vector2 otherPoint = new Vector2(currentBiome.GetTemperature(), currentBiome.GetRainfall());

                float distance = Vector2.Distance(point, otherPoint);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    biome = currentBiome;
                }
            }

            //return blossom;
            return biome;
        }
    }
}
