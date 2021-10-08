using ancient.game.entity;
using ancient.game.world.block;
using ancient.game.world.chunk;
using ancient.game.world.generator;
using ancient.game.world.generator.noise;
using ancientlib.game.entity;
using ancientlib.game.init;
using ancientlib.game.world.chunk;
using ancientlib.game.world.structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.biome
{
    public abstract class Biome
    {
        public static int WATER_LEVEL = 64;
        public static int CLOUDS_LEVEL = 128;

        protected string name;

        protected int minHeight;
        protected int maxHeight;

        protected float elevation;
        protected float roughness;
        protected float detail;

        protected float temperature;
        protected float rainfall;

        protected Block topBlock;
        protected Block baseBlock;

        protected Color grassColor1;
        protected Color grassColor2;

        protected Color skyColor;
        protected Color waterColor;

        protected Dictionary<Tuple<float, float>, Block> decorationBlocks;
        protected List<Type> spawnableEntities;

        protected List<string> bgmList;

        public Biome(string name)
        {
            this.name = name;

            this.maxHeight = 80;
            this.minHeight = 48;

            this.elevation = 0.25F;
            this.roughness = 2;
            this.detail = 8;

            this.topBlock = Blocks.grass;
            this.baseBlock = Blocks.dirt;

            this.grassColor1 = new Color(60, 100, 60);
            this.grassColor2 = new Color(33, 63, 33);

            this.skyColor = Color.CornflowerBlue;
            this.waterColor = Blocks.water.GetColor();

            this.decorationBlocks = new Dictionary<Tuple<float, float>, Block>();
            this.spawnableEntities = new List<Type>();

            this.bgmList = new List<string>();
        }

        public string GetName()
        {
            return this.name;
        }

        public Biome SetName(string name)
        {
            this.name = name;
            return this;
        }

        public int GetMinHeight()
        {
            return this.minHeight;
        }

        public int GetMaxHeight()
        {
            return this.maxHeight;
        }

        public Biome SetHeight(int maxHeight, int minHeight)
        {
            this.maxHeight = maxHeight;
            this.minHeight = minHeight;
            return this;
        }

        public float GetElevation()
        {
            return this.elevation;
        }

        public float GetRoughness()
        {
            return this.roughness;
        }

        public float GetDetail()
        {
            return this.detail;
        }

        public Biome SetHeightMapModifiers(float elevation, float roughness, float detail)
        {
            this.elevation = elevation;
            this.roughness = roughness;
            this.detail = detail;
            return this;
        }

        public float GetTemperature()
        {
            return this.temperature;
        }

        public float GetRainfall()
        {
            return this.rainfall;
        }

        public Biome SetTemperatureRainfall(float temperature, float rainfall)
        {
            this.temperature = temperature;
            this.rainfall = rainfall;
            return this;
        }

        public Block GetTopBlock()
        {
            return this.topBlock;
        }

        public Biome SetTopBlock(Block topBlock)
        {
            this.topBlock = topBlock;
            return this;
        }

        public Block GetBaseBlock()
        {
            return this.baseBlock;
        }

        public Biome SetBaseBlock(Block baseBlock)
        {
            this.baseBlock = baseBlock;
            return this;
        }

        public Color GetGrassColor1()
        {
            return this.grassColor1;
        }

        public Biome SetGrassColor1(Color grassColor1)
        {
            this.grassColor1 = grassColor1;
            return this;
        }

        public Color GetGrassColor2()
        {
            return this.grassColor2;
        }

        public Biome SetGrassColor2(Color grassColor2)
        {
            this.grassColor2 = grassColor2;
            return this;
        }

        public Color GetSkyColor()
        {
            return this.skyColor;
        }

        public Biome SetSkyColor(Color skyColor)
        {
            this.skyColor = skyColor;
            return this;
        }

        public Color GetWaterColor()
        {
            return this.waterColor;
        }

        public Biome SetWaterColor(Color waterColor)
        {
            this.waterColor = waterColor;
            return this;
        }

        public virtual void Generate(Chunk chunk, int x, int z, int height)
        {
            bool genScenery = false;

            BlockArray blocks = chunk.GetBlockArray();

            int wx = x + chunk.GetX();
            int wz = z + chunk.GetZ();

            int xzIndex = x * 16 * 16 + z;

            for (int y = 0; y < 16; y++)
            {
                int index = xzIndex + y * 16;

                int wy = chunk.GetY() + y;

                if (wy <= height)
                {
                    if (wy == height)
                    {
                        blocks.SetBlock(topBlock, index);

                        if (wy > WATER_LEVEL)
                            genScenery = true;
                    }
                    else if (wy == height - 1)
                        blocks.SetBlock(baseBlock, index);
                    else if (wy == 0)
                        blocks.SetBlock(Blocks.baseBlock, index);
                    else
                        blocks.SetBlock(Blocks.stone, index);
                }
                else
                {
                    if (wy <= WATER_LEVEL)
                    {
                        if (wy == WATER_LEVEL && BiomeManager.GetTemperature(x + chunk.GetX(), z + chunk.GetZ()) <= 0.375)
                            blocks.SetBlock(Blocks.ice, index);
                        else
                            blocks.SetBlock(Blocks.water, index);
                    }
                    else
                        GenerateDecorationBlocks(chunk, wy, height, index, x, z);
                }
            }

            if (genScenery)
            {
                GenerateScenery(chunk, x, height + 1, z);
                SpawnEntities(chunk, x, height + 2, z);
            }

            GenerateClouds(chunk, wx, chunk.GetY(), wz);
        }

        public abstract void GenerateScenery(Chunk chunk, int x, int wy, int z);

        private void SpawnEntities(Chunk chunk, int x, int y, int z)
        {
            if (spawnableEntities.Count == 0)
                return;

            if (chunk.rand.Next(4096) != 0)
                return;

            Type type = spawnableEntities[chunk.rand.Next(spawnableEntities.Count)];

            int amount = chunk.rand.Next(4);
            for (int i = 0; i < amount; i++)
            {
                Entity entity = (Entity)Activator.CreateInstance(type, chunk.GetWorld());

                entity.SetX(chunk.GetX() + x);
                entity.SetY(y + entity.GetHeight() + 1);
                entity.SetZ(chunk.GetZ() + z);

                chunk.GetWorld().SpawnEntity(entity);
            }
        }

        public void GenerateClouds(Chunk chunk, int x, int y, int z)
        {
            if (y != CLOUDS_LEVEL)
                return;

            if (chunk.rand.Next(8192) == 0)
                Structures.GetStructureFromName("cloud_" + chunk.rand.Next(2)).Generate(chunk.GetWorld(), x, y, z);
        }

        public virtual void GenerateDecorationBlocks(Chunk chunk, int wy, int height, int index, int x, int z)
        {
            if (wy == height + 1)
            {
                if (decorationBlocks.Count > 0)
                {
                    float chance = (float)chunk.rand.NextDouble();

                    foreach (KeyValuePair<Tuple<float, float>, Block> pair in decorationBlocks)
                    {
                        float min = pair.Key.Item1;
                        float max = pair.Key.Item2;

                        if (min <= chance && max > chance)
                        {
                            chunk.GetBlockArray().SetBlock(pair.Value, index);
                            break;
                        }
                    }
                }
            }
        }

        protected void AddDecorationBlock(float chance, Block block)
        {
            if (decorationBlocks.Count == 0)
                decorationBlocks.Add(new Tuple<float, float>(0, chance), block);
            else
            {
                float min = decorationBlocks.ElementAt(decorationBlocks.Count - 1).Key.Item2;
                decorationBlocks.Add(new Tuple<float, float>(min, min + chance), block);
            }
        }

        public List<string> GetBGMList()
        {
            return this.bgmList;
        }

        public Biome SetBGMList(List<string> bgmList)
        {
            this.bgmList = bgmList;
            return this;
        }

        public virtual float GetFogStart()
        {
            return 0.75F;
        }
    }
}
