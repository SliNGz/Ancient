using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using Microsoft.Xna.Framework;

namespace ancientlib.game.item.equip.special
{
    public class ItemJetpack : ItemSpecial
    {
        public ItemJetpack() : base("Jetpack", null, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        { }

        public override void Use(EntityPlayer player, ItemStack itemStack)
        {
            base.Use(player, itemStack);

            if (player.InWater())
                return;

            if (!player.IsRiding())
                player.SetYVelocity(MathHelper.Clamp(player.GetYVelocity() + 0.25F, 0, 7));
            else
                player.GetMount().SetYVelocity(MathHelper.Clamp(player.GetYVelocity() + 0.17F, 0, 2F));
        }
    }
}
