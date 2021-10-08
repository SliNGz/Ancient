using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.entity;
using ancientlib.game.entity.model;
using ancientlib.game.network.packet.server.world;
using ancientlib.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item
{
    public class Item
    {
        protected string name;
        protected int maxItemStack = 64;

        protected int cooldown;

        protected float renderYaw;
        protected float baseRenderYaw;
        protected float renderPitch;
        protected float baseRenderPitch;
        protected float renderRoll;
        protected float baseRenderRoll;
        protected float renderSpeed;

        protected Vector3 handScale;
        protected Vector3 handOffset;
        protected Vector3 dropScale;

        /* Entity Model Collection for the entity created based on this item */
        protected EntityModelCollection modelCollection;

        public Item(string name)
        {
            this.name = name;
            this.cooldown = 0;

            this.renderYaw = 0;
            this.renderPitch = 0;
            this.renderRoll = 45;
            this.baseRenderYaw = 0;
            this.baseRenderPitch = 0;
            this.baseRenderRoll = 0;
            this.renderSpeed = 5;

            this.handScale = new Vector3(0.01F, 0.01F, 0.01F);
            this.handOffset = new Vector3(0.65f, -0.3f, -1f);
            this.dropScale = new Vector3(0.025F, 0.025F, 0.025F);

            this.modelCollection = new EntityModelCollection(GetModelName(), 0.25F, 0.25F, 0.25F);
        }

        public virtual void Use(EntityPlayer player, ItemStack itemStack)
        {
            player.usingItemInHand = true;
            player.GetWorld().PlaySound(GetUseSound(player.GetWorld()));
        }

        public virtual void UseRightClick(EntityPlayer player)
        { }

        public virtual void OnUseFinish(EntityPlayer player, ItemStack itemStack)
        {
            player.usingItemInHand = false;
        }

        public virtual void OnUseRightFinish(EntityPlayer player, ItemStack itemStack)
        {
            player.usingItemInHand = false;
        }

        public virtual void OnPickup(EntityPlayer player, ItemStack itemStack)
        {
            player.AddItem(itemStack);
            player.GetWorld().PlaySound("pickup_item", 0.2F);
        }

        public virtual bool CanUseItem(EntityPlayer player)
        {
            return true;
        }

        public virtual void OnItemChange(EntityPlayer player, ItemStack lastItemStack, ItemStack newItemStack)
        {
            player.handRenderYaw = 0;
            player.handRenderPitch = 0;
            player.handRenderRoll = 0;
            player.usingItemInHand = false;

            if (lastItemStack != null)
                lastItemStack.ticksUsed = 0;

            player.destroyAnimation = 0;
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetMaxItemStack()
        {
            return this.maxItemStack;
        }

        public virtual string GetUseSound(World world)
        {
            return null;
        }

        public float GetRenderYaw()
        {
            return MathHelper.ToRadians(this.renderYaw);
        }

        public Item SetRenderYaw(float renderYaw)
        {
            this.renderYaw = renderYaw;
            return this;
        }

        public float GetBaseRenderYaw()
        {
            return MathHelper.ToRadians(this.baseRenderYaw);
        }

        public Item SetBaseRenderYaw(float baseRenderYaw)
        {
            this.baseRenderYaw = baseRenderYaw;
            return this;
        }

        public float GetRenderPitch()
        {
            return MathHelper.ToRadians(this.renderPitch);
        }

        public Item SetRenderPitch(float renderPitch)
        {
            this.renderPitch = renderPitch;
            return this;
        }

        public float GetBaseRenderPitch()
        {
            return MathHelper.ToRadians(this.baseRenderPitch);
        }

        public Item SetBaseRenderPitch(float baseRenderPitch)
        {
            this.baseRenderPitch = baseRenderPitch;
            return this;
        }

        public float GetRenderRoll()
        {
            return MathHelper.ToRadians(this.renderRoll);
        }

        public Item SetRenderRoll(float renderRoll)
        {
            this.renderRoll = renderRoll;
            return this;
        }

        public float GetBaseRenderRoll()
        {
            return MathHelper.ToRadians(this.baseRenderRoll);
        }

        public Item SetBaseRenderRoll(float baseRenderRoll)
        {
            this.baseRenderRoll = baseRenderRoll;
            return this;
        }

        public virtual float GetRenderSpeed(EntityPlayer player)
        {
            return this.renderSpeed;
        }

        public Item SetRenderSpeed(float renderSpeed)
        {
            this.renderSpeed = renderSpeed;
            return this;
        }

        public virtual string GetModelName()
        {
            string modelName = name;
            modelName = modelName.ToLower();
            modelName = modelName.Replace(' ', '_');
            return modelName;
        }

        public Vector3 GetHandScale()
        {
            return this.handScale;
        }

        public Item SetHandScale(Vector3 handScale)
        {
            this.handScale = handScale;
            return this;
        }

        public Vector3 GetHandOffset()
        {
            return this.handOffset;
        }

        public Item SetHandOffset(Vector3 handOffset)
        {
            this.handOffset = handOffset;
            return this;
        }

        public Vector3 GetDropModelScale()
        {
            return this.dropScale;
        }

        public Item SetDropScale(Vector3 dropScale)
        {
            this.dropScale = dropScale;
            return this;
        }

        public EntityModelCollection GetModelCollection()
        {
            return this.modelCollection;
        }

        public virtual int GetCooldown(EntityPlayer player)
        {
            return this.cooldown;
        }

        public virtual bool CanBeSpammed()
        {
            return false;
        }
    }
}