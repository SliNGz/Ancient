using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.entity;
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
        protected float renderPitch;
        protected float renderRoll;
        protected float renderSpeed;

        protected Vector3 modelScale;
        protected Vector3 modelOffset;
        protected Vector3 dropScale;
        protected EntityModelState dropModelState;

        public Item(string name)
        {
            this.name = name;
            this.cooldown = 0;

            this.renderYaw = 0;
            this.renderPitch = 0;
            this.renderRoll = 0;
            this.renderSpeed = 5;

            this.modelScale = new Vector3(0.01F, 0.01F, 0.01F);
            this.modelOffset = new Vector3(0.65f, -0.3f, -1f);
            this.dropScale = new Vector3(0.025F, 0.025F, 0.025F);

            this.dropModelState = new EntityModelState(GetModelName(), 0.25F, 0.25F, 0.25F);
        }

        public virtual void Use(EntityPlayer player)
        {
            player.usingItemInHand = true;
            player.GetWorld().PlaySound(GetUseSound(player.GetWorld()));
        }

        public virtual void UseRightClick(EntityPlayer player)
        { }

        public virtual void OnPickup(EntityPlayer player, ItemStack itemStack)
        {
            player.AddItem(itemStack);
            player.GetWorld().PlaySound("pickup_item", 0.2F);
        }

        public virtual bool CanUseItem(EntityPlayer player)
        {
            return true;
        }

        public virtual void OnItemChange(EntityPlayer player)
        {
            player.handYaw = 0;
            player.handPitch = 0;
            player.handRoll = 0;
            player.usingItemInHand = false;
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

        public float GetRenderPitch()
        {
            return MathHelper.ToRadians(this.renderPitch);
        }

        public Item SetRenderPitch(float renderPitch)
        {
            this.renderPitch = renderPitch;
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

        public Vector3 GetModelScale()
        {
            return this.modelScale;
        }

        public Item SetModelScale(Vector3 modelScale)
        {
            this.modelScale = modelScale;
            return this;
        }

        public Vector3 GetModelOffset()
        {
            return this.modelOffset;
        }

        public Item SetModelOffset(Vector3 modelOffset)
        {
            this.modelOffset = modelOffset;
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

        public virtual int GetCooldown(EntityPlayer player)
        {
            return this.cooldown;
        }

        public EntityModelState GetDropModelState()
        {
            return this.dropModelState;
        }
    }
}