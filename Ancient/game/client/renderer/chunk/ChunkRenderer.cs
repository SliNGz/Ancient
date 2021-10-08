using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancient.game.world.chunk;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.chunk
{
    public class ChunkRenderer
    {
        private List<ChunkData> loadedList;
        public List<ChunkData> renderList;
        private List<ChunkData> shadowList;

        public ChunkRenderer()
        {
            this.loadedList = new List<ChunkData>();
            this.renderList = new List<ChunkData>();
            this.shadowList = new List<ChunkData>();

            ThreadUtils.CreateThread("Render Thread", Update, true).Start();
        }

        public void Update()
        {
            while (true)
            {
                lock (loadedList)
                {
                    lock (renderList)
                    {
                        lock (shadowList)
                        {
                            renderList.Clear();
                            shadowList.Clear();

                            foreach (ChunkData chunkData in loadedList)
                            {
                                if (chunkData.ShouldDraw(false))
                                    renderList.Add(chunkData);

                                if (chunkData.ShouldDraw(true))
                                    shadowList.Add(chunkData);
                            }
                        }
                    }
                }

                Thread.Sleep(TimeSpan.FromSeconds(1 / 16F));
            }
        }

        public void DrawSolid()
        {
            lock (renderList)
            {
                foreach (ChunkData chunkData in renderList)
                    chunkData.DrawSolid();
            }
        }

        public void DrawLiquid()
        {
            lock (renderList)
            {
                foreach (ChunkData chunkData in renderList)
                    chunkData.DrawLiquid();
            }
        }

        public void DrawShadowSolid()
        {
            lock (shadowList)
            {
                foreach (ChunkData chunkData in shadowList)
                    chunkData.DrawSolid();
            }
        }

        public void DrawShadowLiquid()
        {
            lock (shadowList)
            {
                foreach (ChunkData chunkData in shadowList)
                    chunkData.DrawLiquid();
            }
        }

        public void DrawMap()
        {
            lock (loadedList)
            {
                foreach (ChunkData chunkData in loadedList)
                    chunkData.DrawSolid();

                foreach (ChunkData chunkData in loadedList)
                    chunkData.DrawLiquid();
            }
        }

        public void AddChunk(Chunk chunk)
        {
            lock (loadedList)
                this.loadedList.Add(new ChunkData(chunk));
        }

        public void RemoveChunk(Chunk chunk)
        {
            lock (loadedList)
                this.loadedList.RemoveAll(chunkData => chunkData.chunk == chunk);
        }

        public void ReloadChunk(Chunk chunk)
        {
            lock (loadedList)
                this.loadedList.Find(chunkData => chunkData.chunk == chunk).Reload();
        }
    }

    public struct ChunkData
    {
        public Chunk chunk;
        public VoxelRendererData data;

        public ChunkData(Chunk chunk)
        {
            this.chunk = chunk;
            this.data = new VoxelRendererData(chunk);
        }

        public void Reload()
        {
            if (data != null)
                data.LoadData(chunk);
        }

        public void DrawSolid()
        {
            if (chunk == null)
                return;

            VoxelRenderer.DrawSolid(data, chunk.GetPosition(), Vector3.Zero, 0, 0, 0);
        }

        public void DrawLiquid()
        {
            if (chunk == null)
                return;

            VoxelRenderer.DrawLiquid(data, chunk.GetPosition(), Vector3.Zero, 0, 0, 0);
        }

        public bool ShouldDraw(bool shadow)
        {
            if (shadow)
            {
                if (!Ancient.ancient.world.GetChunkLoader().IsChunkVisibleShadow(Ancient.ancient.player, chunk.GetIndex()))
                    return false;
            }
            else
            {
                if (!Ancient.ancient.world.GetChunkLoader().IsChunkVisible(Ancient.ancient.player, chunk.GetIndex()))
                    return false;
            }

            int solid = 0;
            int liquid = 0;

            if (data.solidVertexBuffer != null)
                solid = data.solidVertexBuffer.VertexCount;

            if (data.liquidVertexBuffer != null)
                liquid = data.liquidVertexBuffer.VertexCount;

            return solid > 0 || liquid > 0;
        }
    }
}
