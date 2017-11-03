using ancient.game.entity.player;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.generator.metadata;
using ancient.game.world.generator.noise;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ancient.game.world.chunk
{
    public class ChunkManager
    {
        private World world;
        private Dictionary<Vector3, Chunk> chunks;

        public ChunkManager(World world)
        {
            this.world = world;
            this.chunks = new Dictionary<Vector3, Chunk>();
        }

        public void Update()
        {
            lock (chunks)
            {
                foreach (Chunk chunk in chunks.Values)
                {
                    if (!chunk.IsEmpty())
                    {
                        for (int i = 0; i < world.players.Count; i++)
                        {
                            EntityPlayer player = world.players[i];

                            if (player != null)
                            {
                                if (Vector3.Distance(player.GetPosition(), chunk.GetPosition()) <= 48)
                                {
                                    chunk.Update();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool ChunkExists(int x, int y, int z)
        {
            return chunks.ContainsKey(new Vector3(x, y, z));
        }

        public Chunk GetChunk(int x, int y, int z)
        {
            Chunk chunk = null;
            chunks.TryGetValue(new Vector3(x, y, z), out chunk);

            return chunk;
        }

        public Chunk GetChunkFromBlock(int x, int y, int z)
        {
            return GetChunk((int)Math.Floor(x / 16f), (int)Math.Floor(y / 16f), (int)Math.Floor(z / 16f));
        }

        public void AddChunk(int x, int y, int z)
        {
            lock (chunks)
                chunks.Add(new Vector3(x, y, z), new Chunk(world, x * 16, y * 16, z * 16));
        }

        public void RemoveChunk(int x, int y, int z)
        {
            lock (chunks)
                chunks.Remove(new Vector3(x, y, z));
        }

        public Dictionary<Vector3, Chunk> GetChunksDictionary()
        {
            return this.chunks;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                return chunk.GetBlock(x, y, z);

            return null;
        }

        public void SetBlock(Block block, int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                chunk.SetBlock(block, x, y, z);
            else
                world.GetMetadata().AddBlockToMetadata(block, x, y, z);
        }

        public void SetBlockDirectly(Block block, int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
            {
                x = (x % 16 + 16) % 16;
                y = (y % 16 + 16) % 16;
                z = (z % 16 + 16) % 16;

                chunk.GetBlockArray().SetBlock(block, x, y, z);
            }
            else
                world.GetMetadata().AddBlockToMetadata(block, x, y, z);
        }

        public void DestroyBlock(int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                chunk.DestroyBlock(x, y, z);
        }

        public void DestroyBlockDirectly(int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
            {
                x = (x % 16 + 16) % 16;
                y = (y % 16 + 16) % 16;
                z = (z % 16 + 16) % 16;

                chunk.GetBlockArray().DestroyBlock(x, y, z);
            }
        }

        public bool IsSolidAt(int x, int y, int z)
        {
            lock (chunks)
            {
                if (GetBlock(x, y, z) != null)
                    return GetBlock(x, y, z).IsSolid();

                return false;
            }
        }

        public bool IsBlockOfType(BlockType type, int x, int y, int z)
        {
            if (GetBlock(x, y, z) != null)
                return GetBlock(x, y, z).GetBlockType() == type;

            return false;
        }

        public BoundingBox GetChunkBoundingBox(int x, int y, int z)
        {
            Vector3 min = new Vector3(x, y, z) * 16;
            Vector3 max = min + new Vector3(16, 16, 16);

            return new BoundingBox(min, max);
        }

        public BoundingBox GetBlockBoundingBox(int x, int y, int z)
        {
            Block block = GetBlock(x, y, z);
            Vector3 dimensions = Vector3.One;

            if (block != null)
                dimensions = block.GetDimensions();

            Vector3 min = new Vector3(x, y, z);
            Vector3 max = min + dimensions;

            return new BoundingBox(min, max);
        }

        public BoundingBox GetBlockBoundingBox(Block block, int x, int y, int z)
        {
            Vector3 min = new Vector3(x, y, z);
            Vector3 max = min + block.GetDimensions();

            return new BoundingBox(min, max);
        }

        public World GetWorld()
        {
            return this.world;
        }

        public bool IsBlockVisibleAt(int x, int y, int z)
        {
            Block block = GetBlock(x, y, z);

            if (block != null)
            {
                if (block.IsOpaque())
                    return true;

                return false;
            }

            return false;
        }
    }
}