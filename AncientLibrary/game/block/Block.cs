using ancient.game.entity;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.chunk;
using ancientlib.game.block;
using ancientlib.game.entity.world;
using ancientlib.game.init;
using ancientlib.game.item;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public abstract class Block
    {
        protected BlockType type;
        protected string name;

        protected Vector3 dimensions;

        protected float slipperiness;
        protected float speedModifier;

        protected Color color;
        protected Color secondaryColor;

        protected int tickRate;

        protected int lightEmission;

        protected Block(string name, BlockType type)
        {
            this.type = type;
            this.name = name;
            this.dimensions = Vector3.One;
            this.slipperiness = 0;
            this.speedModifier = 1;
            this.color = Color.White;
            this.secondaryColor = Color.White;
            this.tickRate = 0;
            this.lightEmission = 0;
        }

        public BlockType GetBlockType()
        {
            return this.type;
        }

        public string GetName()
        {
            return this.name;
        }

        public Vector3 GetDimensions()
        {
            return dimensions;
        }

        public float GetSlipperiness()
        {
            return this.slipperiness;
        }

        public float GetSpeedModifier()
        {
            return this.speedModifier;
        }

        public Color GetColor()
        {
            return this.color;
        }

        public Color GetSecondaryColor()
        {
            return this.secondaryColor;
        }

        public virtual bool IsOpaque()
        {
            return this.color.A == 255 && this.secondaryColor.A == 255;
        }

        public virtual bool IsTranslucent()
        {
            return this.color.A > 0 && this.color.A < 255 && this.secondaryColor.A > 0 && this.secondaryColor.A < 255;
        }

        public bool IsVisible()
        {
            return this.color.A > 0 && this.secondaryColor.A > 0;
        }

        public virtual bool IsSolid()
        {
            return this.type.IsSolid();
        }

        public virtual void OnCollide(Entity entity, BoundingBox boundingBox)
        {
            entity.SetMaximumSlipperiness(slipperiness);
            entity.SetMinimumSpeedModifier(speedModifier);
        }

        public virtual void OnPlace(World world, int x, int y, int z)
        {
            ReloadNearbyLightSources(world, x, y, z);
        }

        public virtual void OnDestroy(World world, int x, int y, int z)
        {
            ReloadNearbyLightSources(world, x, y, z);
            DropItems(world, x, y, z);
        }

        private void ReloadNearbyLightSources(World world, int x, int y, int z)
        {
            if (lightEmission == 0)
            {
                List<Tuple<int, int, int>> lights = new List<Tuple<int, int, int>>();

                foreach (var light in world.GetLightingManager().lights)
                {
                    if (Vector3.Distance(new Vector3(light.Item1, light.Item2, light.Item3), new Vector3(x, y, z)) <= 15)
                        lights.Add(light);
                }

                foreach (var light in lights)
                    world.ReloadLightSource(light.Item1, light.Item2, light.Item3);
            }
        }

        public virtual void OnTick(World world, int x, int y, int z)
        { }

        public int GetTickRate()
        {
            return this.tickRate;
        }

        public virtual string GetModelName()
        {
            string modelName = name;
            modelName = modelName.ToLower();
            modelName = modelName.Replace(' ', '_');
            return modelName;
        }

        public virtual Vector3 GetItemModelScale()
        {
            return new Vector3(0.1F, 0.1F, 0.1F);
        }

        public int GetLightEmission()
        {
            return this.lightEmission;
        }

        public virtual bool IsFullBlock()
        {
            return true;
        }

        public virtual bool ShouldRenderFace(Block neighbor, int xOffset, int yOffset, int zOffset)
        {
            return this.color.A > neighbor.GetColor().A || !neighbor.IsFullBlock();
        }

        public virtual bool CanBeDestroyed()
        {
            return IsSolid();
        }

        public virtual bool CanBePlaced(World world, int x, int y, int z)
        {
            return true;
        }

        public virtual void OnNeighborDestroyed(World world, int x, int y, int z, Block neighbor)
        { }

        protected virtual bool CanBlockStay(World world, int x, int y, int z)
        {
            return true;
        }

        protected virtual void DropItems(World world, int x, int y, int z)
        {
            DropItem(world, x, y, z, Items.GetItemFromID(Blocks.GetIDFromBlock(this)), 1, 100);
        }

        protected void DropItem(World world, int x, int y, int z, Item item, int amount, double chance)
        {
            if (amount <= 0 || world.rand.NextDouble() >= chance / 100.0)
                return;

            amount = MathHelper.Clamp(amount, 0, item.GetMaxItemStack());

            EntityDrop drop = new EntityDrop(world, x, y, z, item, amount);
            drop.SetPosition(x + 0.5F, y + 0.5F, z + 0.5F);
            drop.SetXVelocity(world.rand.NextDouble() * world.rand.Next(2) == 0 ? -1 : 1);
            drop.SetYVelocity(3);
            drop.SetZVelocity(world.rand.NextDouble() * world.rand.Next(2) == 0 ? -1 : 1);

            world.SpawnEntity(drop);
        }
    }

    public enum BlockFace
    {
        DOWN,
        UP,
        NORTH,
        SOUTH,
        WEST,
        EAST
    }
}