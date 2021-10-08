using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using ancientlib.game.item.projectile;

namespace ancientlib.game.item.weapon
{
    public abstract class ItemProjectileConsumer : ItemWeapon
    {
        public ItemProjectileConsumer(string name, Class _class, int damage, int cooldown) : base(name, _class, damage, cooldown)
        { }

        public override void Use(EntityPlayer player, ItemStack itemStack)
        {
            base.Use(player, itemStack);

            ItemProjectile projectile = player.GetProjectileInUse(this);
            player.RemoveItem(projectile, 1);
        }

        public override bool CanUseItem(EntityPlayer player)
        {
            ItemProjectile projectile = player.GetProjectileInUse(this);

            if (projectile != null)
                return base.CanUseItem(player) && projectile.CanUseItem(player);

            Console.WriteLine("No projectile to use..");
            return false;
        }
    }
}
