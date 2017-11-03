using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ancientlib.game.item.world
{
    public class ItemCoin : Item
    {
        public ItemCoin() : base("Berkin")
        {
            this.maxItemStack = short.MaxValue;
            this.dropScale = Vector3.One * 0.01F;
        }

        public override void OnPickup(EntityPlayer player, ItemStack itemStack)
        {
            player.AddBerkins(itemStack.GetAmount());
            player.GetWorld().PlaySound("pickup_item", 0.2F);
        }
    }
}
