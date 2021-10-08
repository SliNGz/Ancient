using ancient.game.entity.player;
using ancientlib.game.item;
using ancientlib.game.item.equip;
using ancientlib.game.item.equip.bottom;
using ancientlib.game.item.equip.special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.inventory
{
    public class InventoryPlayer : Inventory
    {
        private EntityPlayer player;

        private ItemBottom bottom;
        private ItemSpecial special;

        public InventoryPlayer(EntityPlayer player, int slots) : base(slots)
        {
            this.player = player;
        }

        public void Equip(ItemEquip equip)
        {
            if (equip.CanUseItem(player))
            {
                if (equip is ItemBottom)
                    EquipBottom((ItemBottom)equip);
                else if (equip is ItemSpecial)
                    EquipSpecial((ItemSpecial)equip);
            }
        }

        public ItemBottom GetBottom()
        {
            return this.bottom;
        }

        public void EquipBottom(ItemBottom bottom)
        {
            this.bottom = bottom;
        }

        public ItemSpecial GetSpecial()
        {
            return this.special;
        }

        public void EquipSpecial(ItemSpecial special)
        {
            this.special = special;
        }

        public void UseSpecial()
        {
            if (special == null)
                return;

            if (special.CanUseItem(player))
                special.Use(player, null);
        }
    }
}
