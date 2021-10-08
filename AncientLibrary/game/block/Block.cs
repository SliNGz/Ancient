using ancient.game.entity;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.chunk;
using ancientlib.game.block;
using ancientlib.game.entity.world;
using ancientlib.game.init;
using ancientlib.game.item;
using ancientlib.game.world.biome;
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

        public Block SetName(string name)
        {
            this.name = name;
            return this;
        }

        public Vector3 GetDimensions()
        {
            return this.dimensions;
        }

        public virtual Vector3 GetRenderDimensions(World world, int x, int y, int z)
        {
            return this.dimensions;
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

        public Block SetColor(Color color)
        {
            this.color = color;
            return this;
        }

        public Color GetSecondaryColor()
        {
            return this.secondaryColor;
        }

        public Block SetSecondaryColor(Color secondaryColor)
        {
            this.secondaryColor = secondaryColor;
            return this;
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
        { }

        public virtual void OnDestroy(World world, int x, int y, int z)
        {
            DropItems(world, x, y, z);
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

        public virtual bool ShouldRenderFace(World world, int x, int y, int z, Block neighbor, int xOffset, int yOffset, int zOffset)
        {
            if (yOffset == 1)
            {
                if (GetRenderDimensions(world, x, y, z).Y < 1)
                    return true;
            }

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

        public virtual Color GetColorAt(World world, int x, int y, int z)
        {
            float noise = (float)(world.GetSimplexNoise().Evaluate(x / 32.0, y / 4.0, z / 32.0));
            noise = (noise + 1) / 2;

            return Color.Lerp(color, secondaryColor, noise);
        }

        public virtual Vector4 GetShaderTechnique(World world, int x, int y, int z)
        {
            return Vector4.Zero;
        }
    }
}