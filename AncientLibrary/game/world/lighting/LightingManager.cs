using ancient.game.world;
using ancient.game.world.block;
using ancient.game.world.chunk;
using ancientlib.game.init;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.world.lighting
{
    public class LightingManager
    {
        private World world;

        private Queue<LightNode> lightQueue;
        private Queue<LightNode> lightRemoveQueue;
        private Queue<Tuple<Block, LightNode>> lightReloadQueue;

        public List<Tuple<int, int, int>> lights;

        public LightingManager(World world)
        {
            this.world = world;
            this.lightQueue = new Queue<LightNode>();
            this.lightRemoveQueue = new Queue<LightNode>();
            this.lightReloadQueue = new Queue<Tuple<Block, LightNode>>();
            this.lights = new List<Tuple<int, int, int>>();

            ThreadUtils.CreateThread("Lighting Thread", Update, true).Start();
        }

        private void Update()
        {
            while (true)
            {
                while (lightRemoveQueue.Count > 0)
                {
                    LightNode node = lightRemoveQueue.Dequeue();

                    Chunk chunk = node.chunk;

                    int x = 0;
                    int y = 0;
                    int z = 0;
                    node.GetUnpackedValue(out x, out y, out z);

                    int blocklight = chunk.GetBlocklight(x, y, z);
                    chunk.SetBlocklight(x, y, z, 0);

                    PropagateLightRemoval(x, y - 1, z, blocklight);
                    PropagateLightRemoval(x, y + 1, z, blocklight);
                    PropagateLightRemoval(x, y, z - 1, blocklight);
                    PropagateLightRemoval(x, y, z + 1, blocklight);
                    PropagateLightRemoval(x - 1, y, z, blocklight);
                    PropagateLightRemoval(x + 1, y, z, blocklight);
                }

                while (lightReloadQueue.Count > 0)
                {
                    var tuple = lightReloadQueue.Dequeue();

                    Block lightSource = tuple.Item1;
                    LightNode node = tuple.Item2;

                    int x = 0;
                    int y = 0;
                    int z = 0;
                    node.GetUnpackedValue(out x, out y, out z);

                    AddLightSource(lightSource, x, y, z);
                }

                while (lightQueue.Count > 0)
                {
                    LightNode node = lightQueue.Dequeue();

                    int x = 0;
                    int y = 0;
                    int z = 0;
                    node.GetUnpackedValue(out x, out y, out z);

                    int blocklight = world.GetBlocklight(x, y, z);

                    PropagateLight(x, y - 1, z, blocklight);
                    PropagateLight(x, y + 1, z, blocklight);
                    PropagateLight(x, y, z - 1, blocklight);
                    PropagateLight(x, y, z + 1, blocklight);
                    PropagateLight(x - 1, y, z, blocklight);
                    PropagateLight(x + 1, y, z, blocklight);
                }

                Thread.Sleep(TimeSpan.FromSeconds(1 / 1024.0));
            }
        }

        private void PropagateLight(int x, int y, int z, int neighborBlocklight)
        {
            lock (world.GetChunkManager().GetChunksDictionary())
            {
                Chunk chunk = world.GetChunkFromBlock(x, y, z);

                if (chunk == null)
                    return;

                int blocklight = chunk.GetBlocklight(x, y, z);

                if (blocklight + 1 < neighborBlocklight)
                {
                    chunk.SetBlocklight(x, y, z, neighborBlocklight - 1);

                    if (!chunk.GetBlock(x, y, z).IsOpaque() || !chunk.GetBlock(x, y, z).IsFullBlock())
                        EnqueueLightNode(chunk, x, y, z);
                }
            }
        }

        private void PropagateLightRemoval(int x, int y, int z, int neighborBlocklight)
        {
            lock (world.GetChunkManager().GetChunksDictionary())
            {
                Chunk chunk = world.GetChunkFromBlock(x, y, z);

                if (chunk == null)
                    return;

                int blocklight = chunk.GetBlocklight(x, y, z);

                if (blocklight != 0 && blocklight < neighborBlocklight)
                    EnqueueLightRemoveNode(chunk, x, y, z);
                else if (blocklight >= neighborBlocklight)
                    EnqueueLightNode(chunk, x, y, z);
            }
        }

        private void EnqueueLightNode(Chunk chunk, int x, int y, int z)
        {
            lightQueue.Enqueue(new LightNode(chunk, x, y, z));
        }

        private void EnqueueLightRemoveNode(Chunk chunk, int x, int y, int z)
        {
            lightRemoveQueue.Enqueue(new LightNode(chunk, x, y, z));
        }

        public void AddLightSource(Block block, int x, int y, int z)
        {
            lock (world.GetChunkManager().GetChunksDictionary())
            {
                Chunk chunk = world.GetChunkFromBlock(x, y, z);

                if (chunk == null)
                    return;

                int blocklight = block.GetLightEmission();

                chunk.SetBlocklight(x, y, z, blocklight);
                EnqueueLightNode(chunk, x, y, z);

                this.lights.Add(new Tuple<int, int, int>(x, y, z));
            }
        }

        public void RemoveLightSource(int x, int y, int z)
        {
            lock (world.GetChunkManager().GetChunksDictionary())
            {
                Chunk chunk = world.GetChunkFromBlock(x, y, z);

                if (chunk == null)
                    return;

                EnqueueLightRemoveNode(chunk, x, y, z);
                this.lights.Remove(new Tuple<int, int, int>(x, y, z));
            }
        }

        public void ReloadLightSource(int x, int y, int z)
        {
            lock (world.GetChunkManager().GetChunksDictionary())
            {
                Chunk chunk = world.GetChunkFromBlock(x, y, z);

                if (chunk == null)
                    return;

                Block lightSource = chunk.GetBlock(x, y, z);

                RemoveLightSource(x, y, z);
                lightReloadQueue.Enqueue(new Tuple<Block, LightNode>(lightSource, new LightNode(chunk, x, y, z)));
            }
        }
    }

    struct LightNode
    {
        public Chunk chunk;
        public short packedValue;

        public LightNode(Chunk chunk, int x, int y, int z)
        {
            this.chunk = chunk;
            this.packedValue = 0;

            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            this.packedValue = GetPackedValue(x, y, z);
        }

        public short GetPackedValue(int x, int y, int z)
        {
            short packedValue = 0;

            packedValue |= (short)x;
            packedValue <<= 4;

            packedValue |= (short)y;
            packedValue <<= 4;

            packedValue |= (short)z;

            return packedValue;
        }

        public void GetUnpackedValue(out int x, out int y, out int z)
        {
            z = chunk.GetZ() + (packedValue & 0xF);
            packedValue >>= 4;

            y = chunk.GetY() + (packedValue & 0xF);
            packedValue >>= 4;

            x = chunk.GetX() + (packedValue & 0xF);
            packedValue >>= 4;
        }
    }
}
