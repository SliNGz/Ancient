using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.entity.player;
using ancientlib.game.init;
using ancientlib.game.item;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancientlib.game.world;

namespace ancientlib.game.entity.world
{
    public class EntityDrop : Entity
    {
        private ItemStack itemStack;

        private float yAnim;
        private float elapsedAnimTime;

        public EntityDrop(World world) : base(world)
        {
            SetDimensions(0.25f, 0.25f, 0.25f);
            this.lifeSpan = Utils.TicksInMinute * 3;
            this.fadeInTicks = (int)(Utils.TicksInSecond * 0.2F);
            this.fadeOutTicks = Utils.TicksInSecond * 2;

            this.yawVelocity = 1.5F;

            this.itemStack = new ItemStack(Items.dirt, 1);
        }

        public EntityDrop(World world, double x, double y, double z, ItemStack itemStack) : this(world)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.itemStack = itemStack;
        }

        public EntityDrop(World world, double x, double y, double z, Item item, int amount) : this(world, x, y, z, new ItemStack(item, amount))
        { }

        public EntityDrop(World world, Item item, int amount) : this(world, 0, 0, 0, new ItemStack(item, amount))
        { }

        public override void Update(GameTime gameTime)
        {
            if (world.IsRemote())
                UpdateRemote(gameTime);
            else
                base.Update(gameTime);

            if (!(world is WorldServer))
                UpdateAnimation(gameTime);
        }

        private void UpdateRemote(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateRotation();
            UpdateBoundingBox();
            UpdateOnGround();
            UpdateAlpha();
            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if (!onGround)
            {
                this.yAnim = 0;
                elapsedAnimTime = -MathHelper.PiOver2;
                return;
            }

            elapsedAnimTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedAnimTime >= 0)
            {
                if (elapsedAnimTime >= MathHelper.Pi)
                    elapsedAnimTime = 0;

                this.yAnim = (float)Math.Sin(elapsedAnimTime) / 2;
            }
        }

        protected override bool CanCollideWithEntityBoundingBox(Entity entity)
        {
            return false;
        }

        protected override void OnCollideWithEntity(Entity entity)
        {
            if (ticksExisted <= 32)
                return;

            if (entity is EntityPlayer)
            {
                itemStack.GetItem().OnPickup((EntityPlayer)entity, itemStack);
                world.DespawnEntity(this);
            }
            else if (entity is EntityPet)
            {
                EntityPet pet = (EntityPet)entity;

                if (pet.HasOwner() && pet.GetOwner() is EntityPlayer)
                {
                    itemStack.GetItem().OnPickup((EntityPlayer)pet.GetOwner(), itemStack);
                    world.DespawnEntity(this);
                }
            }
        }

        public ItemStack GetItemStack()
        {
            return itemStack;
        }

        public void SetItemStack(ItemStack itemStack)
        {
            this.itemStack = itemStack;
        }

        public Item GetItem()
        {
            return this.itemStack.GetItem();
        }

        public Vector3 GetAnimationPosition()
        {
            return new Vector3((float)x, (float)y + yAnim, (float)z);
        }

        public override string GetModelName()
        {
            return itemStack.GetItem().GetModelName();
        }

        public override Vector3 GetModelScale()
        {
            return itemStack.GetItem().GetDropModelScale();
        }

        public override Dictionary<Entity, BoundingBox> GetCollidedEntities()
        {
            Dictionary<Entity, BoundingBox> collidedEntities = new Dictionary<Entity, BoundingBox>();

            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (entity != this)
                {
                    if (entity is EntityPlayerBase)
                    {
                        EntityPlayerBase player = (EntityPlayerBase)entity;

                        if (boundingBox.Intersects(player.GetDropBoundingBox()))
                            collidedEntities.Add(player, player.GetDropBoundingBox());
                    }
                    else
                    {
                        if (boundingBox.Intersects(entity.GetBoundingBox()))
                            collidedEntities.Add(entity, entity.GetBoundingBox());
                    }
                }
            }

            return collidedEntities;
        }

        public override bool ShouldSendRotation()
        {
            return false;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            itemStack.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            itemStack.Write(writer);
        }
    }
}
