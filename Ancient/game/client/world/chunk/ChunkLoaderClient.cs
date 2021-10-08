using ancientlib.game.world.chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancient.game.world.chunk;
using ancient.game.entity.player;
using ancient.game.renderers.world;
using System.Diagnostics;
using System.Threading;
using ancient.game.client.renderer.chunk;
using ancientlib.game.utils;

namespace ancient.game.client.world.chunk
{
    class ChunkLoaderClient : ChunkLoader
    {
        private ChunkRenderer chunkRenderer;

        public ChunkLoaderClient(WorldClient world) : base(world)
        {
            this.chunkRenderer = world.GetRenderer().GetChunkRenderer();

            ThreadUtils.CreateThread("Chunk Reload Thread", UpdateReloadSet, true).Start();
        }

        protected override void Update()
        {
            while (true)
            {
                Update(Ancient.ancient.player);
                Thread.Sleep(TimeSpan.FromSeconds(1 / 128.0));
            }
        }

        private void UpdateReloadSet()
        {
            while (true)
            {
                for (int i = 0; i < reloadSet.Count; i++)
                {
                    Vector3 index = reloadSet[i];
                    ReloadChunk((int)index.X, (int)index.Y, (int)index.Z);
                }

                reloadSet.Clear();
                Thread.Sleep(TimeSpan.FromSeconds(1 / 128.0));
            }
        }

        protected override void LoadChunk(int x, int y, int z)
        {
            base.LoadChunk(x, y, z);
            chunkRenderer.AddChunk(world.GetChunk(x, y, z));
        }

        protected override void ReloadChunk(int x, int y, int z)
        {
            chunkRenderer.ReloadChunk(world.GetChunk(x, y, z));
        }

        protected override void UnloadChunk(int x, int y, int z)
        {
            chunkRenderer.RemoveChunk(world.GetChunk(x, y, z));
            base.UnloadChunk(x, y, z);
        }

        public override bool IsChunkVisible(EntityPlayer player, Vector3 index)
        {
            return WorldRenderer.camera.InViewFrustum(world.GetChunkManager().GetChunkBoundingBox((int)index.X, (int)index.Y, (int)index.Z));
        }

        public override bool IsChunkVisibleShadow(EntityPlayer player, Vector3 index)
        {
            return WorldRenderer.camera.InViewFrustum(world.GetChunkManager().GetChunkRenderBoundingBox((int)index.X, (int)index.Z));
        }
    }
}
