using ancient.game.entity.player;
using ancient.game.world;
using ancient.game.world.chunk;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.world.chunk
{
    public class ChunkLoader
    {
        protected World world;
        protected List<EntityPlayer> players;

        protected HashSet<Vector3> buildSet;
        protected HashSet<Vector3> loadSet;
        protected HashSet<Vector3> reloadSet;
        protected HashSet<Vector3> unloadSet;

        public ChunkLoader(World world)
        {
            this.world = world;

            this.buildSet = new HashSet<Vector3>();
            this.loadSet = new HashSet<Vector3>();
            this.reloadSet = new HashSet<Vector3>();
            this.unloadSet = new HashSet<Vector3>();

            PrepareSpawn();
            ThreadUtils.CreateThread("Chunk Loader Thread", Update, true).Start();
        }

        private void PrepareSpawn()
        {
            Vector3 spawnPoint = new Vector3(world.rand.Next(-1000, 1000), 0, world.rand.Next(-1000, 1000));
            Vector3 chunkIndex = Utils.GetChunkPositionFromPosition(spawnPoint);

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    for (int y = 0; y <= World.MAX_HEIGHT / 16; y++)
                    {
                        BuildChunk((int)chunkIndex.X + x, y, (int)chunkIndex.Z + z);
                    }
                }
            }

            spawnPoint.Y = world.GetSurfaceHeight((int)spawnPoint.X, (int)spawnPoint.Z);
            world.SetSpawnPoint(spawnPoint);
        }

        protected virtual void Update()
        {
            while (true)
            {
                for (int i = 0; i < world.players.Count; i++)
                    Update(world.players[i]);

                Thread.Sleep(TimeSpan.FromSeconds(1 / 128.0));
            }
        }

        protected virtual void Update(EntityPlayer player)
        {
            if (player == null)
                return;

            UpdateChunkLoader(player);
            UpdateBuildSet();
            UpdateLoadSet(player);
            UpdateUnloadSet();
        }

        protected void UpdateChunkLoader(EntityPlayer player)
        {
            int buildsPerTick = 0;
            int unloadsPerTick = 0;

            Vector3 playerIndex = new Vector3((int)Math.Floor(player.GetX() / 16), (int)Math.Floor(player.GetY() / 16), (int)Math.Floor(player.GetZ() / 16));

            int renderDistance = player.GetRenderDistance();
            int distance = renderDistance * 2;
            int x = 0, z = 0, dx = 0, dy = -1;
            int t = distance;
            int maxI = t * t;

            for (int i = 0; i < maxI; i++)
            {
                if ((-distance / 2 <= x) && (x <= distance / 2) && (-distance / 2 <= z) && (z <= distance / 2))
                {
                    int xLoad = x + (int)playerIndex.X;
                    int zLoad = z + (int)playerIndex.Z;

                    for (int j = 0; j <= 4; j++)
                    {
                        if (buildsPerTick >= 64)
                        {
                            i = maxI;
                            break;
                        }

                        for (int k = 0; k < 2; j *= -1, k++)
                        {
                            int yLoad = MathHelper.Clamp((int)playerIndex.Y + j, World.MIN_HEIGHT / 16, World.MAX_HEIGHT / 16 - 1);
                            Vector3 chunkIndex = new Vector3(xLoad, yLoad, zLoad);

                            if (Vector3.Distance(chunkIndex, playerIndex) > player.GetRenderDistance())
                                continue;

                            if (world.ChunkExists(xLoad, yLoad, zLoad))
                                continue;

                            buildSet.Add(chunkIndex);
                            buildsPerTick++;
                        }
                    }
                }

                if ((x == z) || ((x < 0) && (x == -z)) || ((x > 0) && (x == 1 - z)))
                {
                    t = dx; dx = -dy; dy = t;
                }
                x += dx; z += dy;
            }

            foreach (Vector3 chunkIndex in world.GetChunkManager().GetChunksDictionary().Keys)
            {
                if (unloadsPerTick > 16)
                    break;

                if (Vector3.Distance(chunkIndex, playerIndex) > player.GetRenderDistance() + 1)
                {
                    unloadSet.Add(chunkIndex);
                    unloadsPerTick++;
                }
            }
        }

        protected void UpdateBuildSet()
        {
            foreach (Vector3 index in buildSet)
                BuildChunk((int)index.X, (int)index.Y, (int)index.Z);

            buildSet.Clear();
        }

        protected void UpdateLoadSet(EntityPlayer player)
        {
            HashSet<Vector3> newLoadSet = new HashSet<Vector3>();

            foreach (Vector3 index in loadSet)
            {
                if (!ShouldLoad(player, index) || !IsChunkVisible(player, index))
                {
                    newLoadSet.Add(index);
                    continue;
                }

                LoadChunk((int)index.X, (int)index.Y, (int)index.Z);
            }

            loadSet = newLoadSet;
        }

        protected void UpdateUnloadSet()
        {
            foreach (Vector3 index in unloadSet)
                UnloadChunk((int)index.X, (int)index.Y, (int)index.Z);

            unloadSet.Clear();
        }

        protected virtual void BuildChunk(int x, int y, int z)
        {
            world.AddChunk(x, y, z);
            loadSet.Add(new Vector3(x, y, z));
        }

        protected virtual void LoadChunk(int x, int y, int z)
        {
            world.GetChunk(x, y, z).Load();
        }

        protected virtual void ReloadChunk(int x, int y, int z)
        { }

        protected virtual void UnloadChunk(int x, int y, int z)
        {
            world.RemoveChunk(x, y, z);
        }

        protected bool ShouldLoad(EntityPlayer player, Vector3 index)
        {
            Chunk chunk = world.GetChunk((int)index.X, (int)index.Y, (int)index.Z);

            if (chunk == null)
                return false;

            Chunk[] neighbors = chunk.GetNeighbors();
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (index.Y == 0 && i == 0)
                    continue;

                if (index.Y == World.MAX_HEIGHT - 1 && i == 1)
                    continue;

                if (neighbors[i] == null)
                    return false;
            }

            return true;
        }

        public virtual bool IsChunkVisible(EntityPlayer player, Vector3 index)
        {
            return true;
        }

        public void AddToReloadSet(Vector3 index)
        {
            lock (reloadSet)
            {
                if (!reloadSet.Contains(index))
                    this.reloadSet.Add(index);
            }
        }
    }
}
