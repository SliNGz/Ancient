using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.chunk;
using ancient.game.world.generator;
using ancient.game.world.generator.metadata;
using ancient.game.world.generator.noise;
using ancientlib.game.block;
using ancientlib.game.entity;
using ancientlib.game.entity.ai.pathfinding;
using ancientlib.game.init;
using ancientlib.game.item;
using ancientlib.game.network.packet.server.world;
using ancientlib.game.particle;
using ancientlib.game.utils;
using ancientlib.game.utils.chat;
using ancientlib.game.world;
using ancientlib.game.world.biome;
using ancientlib.game.world.chunk;
using ancientlib.game.world.entity;
using ancientlib.game.world.lighting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace ancient.game.world
{
    public abstract class World
    {
        public static World world;

        protected int seed;
        public Random rand;

        public static float GRAVITY = 9.81f;
        public static readonly int MIN_HEIGHT = 0;
        public static readonly int MAX_HEIGHT = 256;

        private Metadata metadata;

        protected ChunkManager chunkManager;
        protected ChunkLoader chunkLoader;

        protected SimplexNoise simplexNoise;

        public EntityList entityList;
        public PlayerList players;

        protected Vector3 spawnPoint;

        public int sunlight = 15;

        public int dayTicks;
        public Color skyColor;

        public static Color dawnSkyColor = Color.LightBlue;
        public static Color nightSkyColor = Color.Black;

        protected int rainChance = 7;
        protected int rainDuration = Utils.TicksInMinute * 5;
        protected int rainTicks;
        protected bool isRaining = false;
        protected bool wasRaining;

        public LightingManager lightingManager;

        private PathFinderManager pathFinderManager;

        public World(int seed)
        {
            this.seed = seed;
            this.rand = new Random(seed);

            this.metadata = new Metadata();

            this.simplexNoise = new SimplexNoise(seed);
            TerrainGenerator.heightNoise = this.simplexNoise;

            this.chunkManager = new ChunkManager(this);

            this.entityList = new EntityList(this);
            this.players = new PlayerList(this);

            this.skyColor = dawnSkyColor;
            this.dayTicks = 128 * 60 * 5;

            this.wasRaining = !isRaining;

            this.lightingManager = new LightingManager(this);

            this.pathFinderManager = new PathFinderManager();

            world = this;
        }

        public virtual void Update(GameTime gameTime)
        {
            lightingManager.Update();
            entityList.Update(gameTime);
            players.Update();
            chunkManager.Update();
          //  UpdateWeather();
           // pathFinderManager.Update();
        }

        public bool ChunkExists(int x, int y, int z)
        {
            return this.chunkManager.ChunkExists(x, y, z);
        }

        public Chunk GetChunk(int x, int y, int z)
        {
            return this.chunkManager.GetChunk(x, y, z);
        }

        public void AddChunk(int x, int y, int z)
        {
            this.chunkManager.AddChunk(x, y, z);
        }

        public void RemoveChunk(int x, int y, int z)
        {
            this.chunkManager.RemoveChunk(x, y, z);
        }

        public Chunk GetChunkFromBlock(int x, int y, int z)
        {
            return this.chunkManager.GetChunkFromBlock(x, y, z);
        }

        public Chunk GetChunkFromBlock(Chunk chunk, int x, int y, int z)
        {
            return this.chunkManager.GetChunkFromBlock(chunk.GetX() + x, chunk.GetY() + y, chunk.GetZ() + z);
        }

        public BoundingBox GetChunkBoundingBox(int x, int y, int z)
        {
            return this.chunkManager.GetChunkBoundingBox(x, y, z);
        }

        public Block GetBlock(int x, int y, int z)
        {
            return this.chunkManager.GetBlock(x, y, z);
        }

        public void SetBlock(Block block, int x, int y, int z)
        {
            this.chunkManager.SetBlock(block, x, y, z);
        }

        /// <summary>
        /// Sets block directly in the chunks' blocks array, used to avoid additional computing.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetBlockDirectly(Block block, int x, int y, int z)
        {
            this.chunkManager.SetBlockDirectly(block, x, y, z);
        }

        public void DestroyBlock(int x, int y, int z)
        {
            this.chunkManager.DestroyBlock(x, y, z);
        }

        /// <summary>
        /// Destroys block directly in the chunks' blocks array, used to avoid additional computing.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void DestroyBlockDirectly(int x, int y, int z)
        {
            this.chunkManager.DestroyBlockDirectly(x, y, z);
        }

        public bool IsSolidAt(int x, int y, int z)
        {
            return this.chunkManager.IsSolidAt(x, y, z);
        }

        public bool IsBlockOfType(BlockType type, int x, int y, int z)
        {
            return this.chunkManager.IsBlockOfType(type, x, y, z);
        }

        public BoundingBox GetBlockBoundingBox(int x, int y, int z)
        {
            return this.chunkManager.GetBlockBoundingBox(x, y, z);
        }

        public BoundingBox GetBlockBoundingBox(Block block, int x, int y, int z)
        {
            return this.chunkManager.GetBlockBoundingBox(block, x, y, z);
        }

        public int GetSurfaceHeight(int x, int z)
        {
            for (int i = 0; i < 16; i++)
            {
                Chunk chunk = chunkManager.GetChunk((int)Math.Floor(x / 16f), i, (int)Math.Floor(z / 16f));

                if (chunk != null)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        if (!chunk.GetBlock(x, y, z).IsSolid())
                            return i * 16 + y;
                    }
                }
            }

            return MAX_HEIGHT;
        }

        public ChunkManager GetChunkManager()
        {
            return this.chunkManager;
        }

        public ChunkLoader GetChunkLoader()
        {
            return this.chunkLoader;
        }

        public int GetSeed()
        {
            return this.seed;
        }

        public Vector3 GetSpawnPoint()
        {
            return this.spawnPoint;
        }

        public void SetSpawnPoint(Vector3 spawnPoint)
        {
            this.spawnPoint = spawnPoint;
        }

        public SimplexNoise GetSimplexNoise()
        {
            return this.simplexNoise;
        }

        public Entity GetEntityFromID(int id)
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                Entity entity = entityList[i];

                if (entity.GetID() == id)
                    return entity;
            }

            return null;
        }

        public virtual void SpawnEntity(Entity entity)
        {
            if (entity is EntityPlayer)
            {
                entity.SetPosition(spawnPoint + new Vector3(0, entity.GetHeight() + 0.5F, 0));
                players.Add((EntityPlayer)entity);
            }

            entityList.Add(entity);
        }

        public virtual void DespawnEntity(Entity entity)
        {
            if (entity is EntityPlayer)
                players.RemovePlayer((EntityPlayer)entity);

            entityList.RemoveEntity(entity);
        }

        public void DespawnEntity(int id)
        {
            Entity entity = GetEntityFromID(id);

            if (entity != null)
                DespawnEntity(entity);
        }

        public virtual void OnDespawnEntity(Entity entity)
        {
            if (IsRemote())
                return;

            if (entity is EntityLiving)
                ((EntityLiving)entity).OnDeath();
        }

        public int GetSunlight()
        {
            return this.sunlight;
        }

        public int GetSunlight(int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                return chunk.GetSunlight(x, y, z);
            else
                return 0;
        }

        public void SetSunlight(int x, int y, int z, int sunlight)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                chunk.SetSunlight(x, y, z, sunlight);
        }

        public int GetBlocklight(int x, int y, int z)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                return chunk.GetBlocklight(x, y, z);
            else
                return 0;
        }

        public void SetBlocklight(int x, int y, int z, int blocklight)
        {
            Chunk chunk = GetChunkFromBlock(x, y, z);

            if (chunk != null)
                chunk.SetBlocklight(x, y, z, blocklight);
        }

        public virtual void PlaySound(string name)
        { }

        public virtual void PlaySound(string name, float volume)
        { }

        public Color GetSkyColor()
        {
            //return GetMyPlayer().GetBiome().GetSkyColor();
            return this.skyColor;
        }

        private void UpdateWeather()
        {
            if (!world.IsRemote())
            {
                rainTicks++;

                if (!isRaining)
                {
                    if (rainTicks % Utils.TicksInMinute * 5 == 0)
                    {
                        if (rand.Next(100) >= rainChance)
                            return;

                        rainDuration = rand.Next(Utils.TicksInMinute * 5, Utils.TicksInMinute * 20);
                        isRaining = true;

                        rainTicks = 0;
                    }
                }
                else
                {
                    rainDuration--;

                    if (rainDuration <= 0)
                        isRaining = false;
                }
            }

            if (wasRaining != isRaining)
                OnWeatherChange();

            this.wasRaining = isRaining;
        }

        protected virtual void OnWeatherChange()
        {
            if (isRaining)
                this.skyColor = Color.Gray;
            else
            {
                //    if (GetMyPlayer() != null)
                //    this.skyColor = GetMyPlayer().GetBiome().GetSkyColor();
            }
        }

        public bool IsRaining()
        {
            return this.isRaining;
        }

        public void SetRaining(bool isRaining)
        {
            this.isRaining = isRaining;
        }

        public virtual void DisplayGui(string name)
        { }

        public virtual void Display3DText(string text, Vector3 position, Vector3 velocity, Color color, float size = 3, int spacing = 1, float maxDistance = -1, int lifeSpan = 0, float sizeChange = 0, float hueChange = 0)
        { }

        public Tuple<int, int, int, Block>[] GetNeighborsOfBlock(int x, int y, int z)
        {
            return new Tuple<int, int, int, Block>[]
            {
                new Tuple<int, int, int, Block>(x, y - 1, z, GetBlock(x, y - 1, z)),
                new Tuple<int, int, int, Block>(x, y + 1, z, GetBlock(x, y + 1, z)),
                new Tuple<int, int, int, Block>(x, y, z - 1, GetBlock(x, y, z - 1)),
                new Tuple<int, int, int, Block>(x, y, z + 1, GetBlock(x, y, z + 1)),
                new Tuple<int, int, int, Block>(x - 1, y, z, GetBlock(x - 1, y, z)),
                new Tuple<int, int, int, Block>(x + 1, y, z, GetBlock(x + 1, y, z)),
            };
        }

        public Metadata GetMetadata()
        {
            return this.metadata;
        }

        public virtual bool IsRemote()
        {
            return false;
        }

        public virtual EntityPlayer GetMyPlayer()
        {
            return null;
        }

        public virtual void OnPlayerChangeItemInHand(ItemStack hand)
        { }

        public virtual void SpawnParticle(Particle particle)
        { }

        public virtual void DespawnParticle(Particle particle)
        { }

        public void CreateExplosion(int x, int y, int z, float a, float b, float c)
        {
            int explosionSize = (int)Math.Round((a + b + c) / 3F);

            for (int i = (int)-a; i <= a; i++)
            {
                for (int j = (int)-b; j <= b; j++)
                {
                    for (int k = (int)-c; k <= c; k++)
                    {
                        float x1 = (i * i) / (a * a);
                        float y1 = (j * j) / (b * b);
                        float z1 = (k * k) / (c * c);

                        if (x1 + y1 + z1 <= 1)
                        {
                            Block block = GetBlock(x + i, y + j, z + k);

                            if (!(block is BlockWater))
                                SetBlock(Blocks.air, x + i, y + j, z + k);

                            if (block != null && !(block is IBlockModel) && !(block is BlockAir))
                            {
                                ParticleVoxel voxel = new ParticleVoxel(this);
                                voxel.SetPosition(new Vector3(x + i, y + j, z + k));

                                Vector3 velocity = new Vector3(i, j, k);
                                velocity /= velocity.Length();
                                velocity *= rand.Next(explosionSize, explosionSize * 3);
                                velocity.Y += explosionSize * 3;

                                voxel.SetVelocity(velocity);
                                voxel.gravity = GRAVITY;
                                voxel.SetRotationVelocity(Vector3.One * (float)rand.NextDouble() * 7F);
                                voxel.SetScale(Vector3.One * rand.Next(20, 100) / 100F);
                                voxel.SetColor(block.GetColor());
                                voxel.SetEndColor(block.GetColor());

                                SpawnParticle(voxel);
                            }
                        }
                    }
                }
            }

            PlaySound("explosion_" + rand.Next(0));

            if (this is WorldServer)
                ((WorldServer)this).BroadcastPacket(new PacketExplosion(x, y, z, a, b, c));
        }

        public abstract void AddChatComponent(ChatComponent chatComponent);

        public Biome GetBiomeAt(int x, int z)
        {
            return BiomeManager.GetBiomeAt(x, z);
        }

        public PathFinderManager GetPathFinderManager()
        {
            return this.pathFinderManager;
        }
    }
}