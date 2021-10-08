using ancientlib.game.constants;
using ancientlib.game.item;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.inventory
{
    public class Inventory
    {
        private ItemStack[] items;
        private int slots; // amount of slots in the inventory
        private int lineSize; // Items in each line

        public Inventory(int slots)
        {
            this.items = new ItemStack[slots];
            this.slots = slots;
            this.lineSize = 6;
        }

        public Inventory(int slots, int lineSize)
        {
            this.items = new ItemStack[slots];
            this.slots = slots;
            this.lineSize = lineSize;
        }

        public int GetSlots()
        {
            return this.slots;
        }

        public void SetSlots(int slots)
        {
            this.slots = MathHelper.Clamp(slots, 1, GameConstants.INV_MAX_SLOTS);

            ItemStack[] items = new ItemStack[slots];
            this.items.CopyTo(items, 0);
            this.items = items;
        }

        public void AddSlots(int add)
        {
            SetSlots(this.slots + add);
        }

        public int GetLineSize()
        {
            return this.lineSize;
        }

        public void SetLineSize(int lineSize)
        {
            this.lineSize = MathHelper.Clamp(lineSize, 1, GameConstants.INV_MAX_LINE_SIZE);
        }

        public void AddLineSize(int add)
        {
            SetLineSize(this.lineSize + add);
        }

        public ItemStack[] GetItems()
        {
            return this.items;
        }

        public Item GetItemAt(int slot)
        {
            ItemStack stack = GetItemStackAt(slot);

            if (stack != null)
                return stack.GetItem();

            return null;
        }

        public void SetItemAt(ItemStack itemStack, int slot)
        {
            if (itemStack.GetAmount() == 0)
                return;

            if (slot < 0 || slot >= this.slots)
                return;

            this.items[slot] = itemStack;
        }

        public bool AddItemUnsafe(Item item, int amount)
        {
            if (amount == 0)
                return false;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = new ItemStack(item, amount);

                    int leftover = amount - item.GetMaxItemStack();

                    if (leftover <= 0)
                        return true;
                    else
                        amount = leftover;
                }
                else
                {
                    if (items[i].GetItem() == item)
                    {
                        int leftover = items[i].TryAddAmount(amount);

                        if (leftover == 0)
                            return true;
                        else
                            amount = leftover;
                    }
                }
            }

            return false;
        }

        public bool AddItemUnsafe(ItemStack itemStack)
        {
            return AddItemUnsafe(itemStack.GetItem(), itemStack.GetAmount());
        }

        public bool RemoveItemUnsafe(Item item, int amount)
        {
            if (amount == 0)
                return false;

            for (int i = items.Length - 1; i >= 0; i--)
            {
                if (items[i] != null)
                {
                    if (items[i].GetItem() == item)
                    {
                        int leftover = items[i].TryAddAmount(-amount);

                        if (items[i].GetAmount() == 0)
                            items[i] = null;

                        if (leftover == 0)
                            return true;
                        else
                            amount = leftover;
                    }
                }
            }

            return false;
        }

        public bool RemoveItemUnsafe(ItemStack itemStack)
        {
            return RemoveItemUnsafe(itemStack.GetItem(), itemStack.GetAmount());
        }

        public int GetSlotOfItem(Item item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == item)
                    return i;
            }

            return -1;
        }

        public ItemStack GetItemStackAt(int slot)
        {
            slot = MathHelper.Clamp(slot, 0, this.slots - 1);

            return items[slot];
        }
    }
}
