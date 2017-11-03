using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item
{
    public class ItemStack
    {
        private Item item;
        private int amount;

        public int ticksElapsed;

        public ItemStack(Item item)
        {
            this.item = item;
            this.amount = 1;
        }

        public ItemStack(Item item, int amount)
        {
            this.item = item;
            SetAmount(amount);
        }

        public Item GetItem()
        {
            return this.item;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public int GetAmount()
        {
            return this.amount;
        }

        public void SetAmount(int amount)
        {
            this.amount = MathHelper.Clamp(amount, 0, item.GetMaxItemStack());
        }

        public void AddAmount(int add)
        {
            SetAmount(this.amount + add);
        }

        public int TryAddAmount(int add)
        {
            int maxAddOrRemove = 0;

            if (add > 0)
                maxAddOrRemove = item.GetMaxItemStack() - amount;
            else
                maxAddOrRemove = amount;

            int leftover = MathHelper.Clamp((add < 0 ? -add : add) - maxAddOrRemove, 0, item.GetMaxItemStack());

            AddAmount(add);

            return leftover;
        }

        public void Use(EntityPlayer player)
        {
            if (CanUseItem(player))
            {
                this.ticksElapsed = GetCooldown(player);
                item.Use(player);
            }
        }

        public void UseRightClick(EntityPlayer player)
        {
            if (CanUseItem(player))
            {
                this.ticksElapsed = GetCooldown(player);
                item.UseRightClick(player);
            }
        }

        public void OnPickup(EntityPlayer player)
        {
            item.OnPickup(player, this);
        }

        public void Update()
        {
            this.ticksElapsed--;
        }

        public bool CanUseItem(EntityPlayer player)
        {
            return this.ticksElapsed <= 0 && item.CanUseItem(player);
        }

        public void OnItemChange(EntityPlayer player)
        {
            this.item.OnItemChange(player);
        }

        public string GetUseSound(World world)
        {
            return this.item.GetUseSound(world);
        }

        public virtual float GetRenderYaw()
        {
            return this.item.GetRenderYaw();
        }

        public virtual float GetRenderPitch()
        {
            return this.item.GetRenderPitch();
        }

        public virtual float GetRenderRoll()
        {
            return this.item.GetRenderRoll();
        }

        public virtual float GetRenderSpeed(EntityPlayer player)
        {
            return this.item.GetRenderSpeed(player);
        }

        public virtual Vector3 GetModelScale()
        {
            return this.item.GetModelScale();
        }

        public virtual Vector3 GetDropModelScale()
        {
            return this.item.GetDropModelScale();
        }

        public int GetCooldown(EntityPlayer player)
        {
            return this.item.GetCooldown(player);
        }

        public void Read(BinaryReader reader)
        {
            int itemID = reader.ReadInt32();
            int amount = reader.ReadInt32();

            this.item = Items.GetItemFromID(itemID);
            SetAmount(amount);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Items.GetIDFromItem(item));
            writer.Write(amount);
        }
    }
}
