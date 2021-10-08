using Microsoft.Xna.Framework;
using ancient.game.world.block;
using System.Collections.Generic;
using ancientlib.game.world.chunk;
using System;
using ancient.game.world.generator;
using ancientlib.game.init;
using ancientlib.game.world;
using ancientlib.game.network;
using ancientlib.game.network.packet.server.world;
using ancientlib.game.world.lighting;
using ancientlib.game.block;

namespace ancient.game.world.chunk
{
    public class Chunk
    {
        private World world;

        private int x;
        private int y;
        private int z;

        private BlockArray blocks;

        private LightMap lightMap;

        public bool isLoaded = false;

        public Random rand;

        private Queue<LightNode> sunlightQueue;

        public Chunk(World world, int x, int y, int z)
        {
            this.world = world;

            this.x = x;
            this.y = y;
            this.z = z;

            this.blocks = new BlockArray(this);

            this.lightMap = new LightMap(this);

            this.rand = new Random(16 * 16 * x + 16 * y + z);

            this.sunlightQueue = new Queue<LightNode>();

            Generate();
        }

        private void Generate()
        {
            TerrainGenerator.Generate(this);

            world.GetMetadata().LoadMetadata(this);
        }

        public void Load()
        {
            //    SetupLighting();
            isLoaded = true;

      /*      for (int x = 0; x < 4096; x++)
            {
                lightMap.SetSunlight(x, 15);
            }*/
        }

        private void SetupLighting()
        {
            Vector3 index = GetIndex();
            Chunk topChunk = world.GetChunk((int)index.X, (int)index.Y + 1, (int)index.Z);

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    int light = topChunk == null ? 15 : topChunk.GetSunlight(x, 0, z);

                    if (light <= 0)
                        continue;

                    lightMap.SetSunlight(x, 15, z, light);
                    sunlightQueue.Enqueue(new LightNode(x, 15, z, light));
                }
            }

            while (sunlightQueue.Count > 0)
            {
                LightNode lightNode = sunlightQueue.Dequeue();

                int x = lightNode.X;
                int y = lightNode.Y;
                int z = lightNode.Z;
                int lightValue = lightNode.LightValue;

                PropagteLight(x, y - 1, z, lightValue);
                PropagteLight(x, y, z - 1, lightValue - 1);
                PropagteLight(x, y, z + 1, lightValue - 1);
                PropagteLight(x - 1, y, z, lightValue - 1);
                PropagteLight(x + 1, y, z, lightValue - 1);
            }
        }

        private void PropagteLight(int x, int y, int z, int lightValue, bool fromNeighborChunk = false)
        {
            if (!InChunkBounds(x, y, z))
            {
                int x1 = ((this.x + x) % 16 + 16) % 16;
                int y1 = ((this.y + y) % 16 + 16) % 16;
                int z1 = ((this.z + z) % 16 + 16) % 16;

                world.GetChunkFromBlock(this.x + x, this.y + y, this.z + z).PropagteLight(x1, y1, z1, lightValue, true);
                return;
            }

            if (lightValue > 0 && lightMap.GetSunlight(x, y, z) < lightValue)
            {
                lightMap.SetSunlight(x, y, z, lightValue);

                if (!fromNeighborChunk)
                {
                    Block block = blocks.GetBlock(x, y, z);

                    if (!block.IsOpaque() || !block.IsFullBlock())
                        sunlightQueue.Enqueue(new LightNode(x, y, z, lightValue));
                }
            }
        }

        public void Update()
        {
            blocks.Update();
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

        public Vector3 GetPosition()
        {
            return new Vector3(x, y, z);
        }

        public Vector3 GetCenter()
        {
            return new Vector3(x + 7, y + 7, z + 7);
        }

        public Vector3 GetIndex()
        {
            return new Vector3((int)Math.Floor(x / 16f), (int)Math.Floor(y / 16f), (int)Math.Floor(z / 16f));
        }

        public BlockArray GetBlockArray()
        {
            return this.blocks;
        }

        public void SetBlockArray(BlockArray blocks)
        {
            this.blocks = blocks;
        }

        public Block GetBlock(int x, int y, int z)
        {
            return blocks.GetBlock((x % 16 + 16) % 16, (y % 16 + 16) % 16, (z % 16 + 16) % 16);
        }

        public void SetBlock(Block block, int x, int y, int z)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            this.blocks.SetBlock(block, x, y, z);

            if (isLoaded)
            {
                if (!world.IsRemote())
                    block.OnPlace(world, this.x + x, this.y + y, this.z + z);

                if (!(world is WorldServer))
                {
                    if (world.GetChunkLoader() != null)
                    {
                        world.GetChunkLoader().AddToReloadSet(GetIndex());
                        AddNeighborsToReloadSet(x, y, z);
                    }
                }
            }
        }

        public void DestroyBlock(int x, int y, int z)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            Block block = GetBlock(x, y, z);
            this.blocks.DestroyBlock(x, y, z);

            if (isLoaded)
            {
                if (!world.IsRemote())
                {
                    block.OnDestroy(world, this.x + x, this.y + y, this.z + z);

                    Tuple<int, int, int, Block>[] neighbors = world.GetNeighborsOfBlock(this.x + x, this.y + y, this.z + z);
                    for (int i = 0; i < 6; i++)
                    {
                        if (neighbors[i].Item4 != null)
                            neighbors[i].Item4.OnNeighborDestroyed(world, neighbors[i].Item1, neighbors[i].Item2, neighbors[i].Item3, block);
                    }

                    if (world is WorldServer)
                        ((WorldServer)world).BroadcastPacket(new PacketDestroyBlock(this.x + x, this.y + y, this.z + z));
                }

                if (!(world is WorldServer))
                {
                    if (world.GetChunkLoader() != null)
                    {
                        world.GetChunkLoader().AddToReloadSet(GetIndex());
                        AddNeighborsToReloadSet(x, y, z);
                    }
                }
            }
        }

        public void AddNeighborsToReloadSet(int x, int y, int z)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            if (y == 0)
                world.GetChunkLoader().AddToReloadSet(GetIndex() + Vector3.Down);
            else if (y == 15)
                world.GetChunkLoader().AddToReloadSet(GetIndex() + Vector3.Up);

            if (z == 0)
                world.GetChunkLoader().AddToReloadSet(GetIndex() + Vector3.Forward);
            else if (z == 15)
                world.GetChunkLoader().AddToReloadSet(GetIndex() + Vector3.Backward);

            if (x == 0)
                world.GetChunkLoader().AddToReloadSet(GetIndex() + Vector3.Left);
            else if (x == 15)
                world.GetChunkLoader().AddToReloadSet(GetIndex() + Vector3.Right);
        }

        public World GetWorld()
        {
            return this.world;
        }

        public Chunk[] GetNeighbors()
        {
            Chunk down = world.GetChunk((int)GetIndex().X, (int)GetIndex().Y - 1, (int)GetIndex().Z);
            Chunk up = world.GetChunk((int)GetIndex().X, (int)GetIndex().Y + 1, (int)GetIndex().Z);
            Chunk north = world.GetChunk((int)GetIndex().X, (int)GetIndex().Y, (int)GetIndex().Z - 1);
            Chunk south = world.GetChunk((int)GetIndex().X, (int)GetIndex().Y, (int)GetIndex().Z + 1);
            Chunk west = world.GetChunk((int)GetIndex().X - 1, (int)GetIndex().Y, (int)GetIndex().Z);
            Chunk east = world.GetChunk((int)GetIndex().X + 1, (int)GetIndex().Y, (int)GetIndex().Z);

            return new Chunk[] { down, up, north, south, west, east };
        }

        public bool IsEmpty()
        {
            return this.blocks.GetNumberOfBlocks() == 0;
        }

        public int GetSurfaceHeight(int x, int z)
        {
            x = (x % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            for (int y = 0; y < 16; y++)
            {
                if (GetBlock(x, y, z) is BlockAir)
                {
                    if (y == 0)
                        return -1;

                    return y;
                }
            }

            return 16;
        }

        public int GetSunlight(int x, int y, int z)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            return this.lightMap.GetSunlight(x, y, z);
        }

        public void SetSunlight(int x, int y, int z, int sunlight)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            this.lightMap.SetSunlight(x, y, z, sunlight);

            world.GetChunkLoader().AddToReloadSet(GetIndex());
        }

        public int GetBlocklight(int x, int y, int z)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            return this.lightMap.GetBlocklight(x, y, z);
        }

        public void SetBlocklight(int x, int y, int z, int blocklight)
        {
            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            this.lightMap.SetBlocklight(x, y, z, blocklight);

            //       world.GetChunkLoader().AddToReloadSet(GetIndex());
        }

        public LightMap GetLightMap()
        {
            return this.lightMap;
        }

        public bool InChunkBounds(int x, int y, int z)
        {
            return x >= 0 && x < 16 && y >= 0 && y < 16 && z >= 0 && z < 16;
        }
    }
}