using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.classes;
using ancientlib.game.entity.projectile;
using ancientlib.game.item.projectile;
using ancientlib.game.item.weapon;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item
{
    public class ItemBow : ItemProjectileConsumer
    {
        public ItemBow(string name, int damage, int cooldown) : base(name, Classes.bowman, damage, cooldown)
        {
            this.renderRoll = -25;
        }

        public override void Use(EntityPlayer player, ItemStack itemStack)
        {
            player.GetWorld().SpawnEntity(new EntityExplosiveArrow(player.GetWorld(), player, (ItemArrow)player.GetProjectileInUse(this)));
            base.Use(player, itemStack);
        }

        public override string GetUseSound(World world)
        {
            return "shoot_arrow_" + world.rand.Next(4);
        }
    }
}
