using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.classes;
using ancientlib.game.item.projectile;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;
using ancientlib.game.entity.projectile;
using ancientlib.game.init;

namespace ancientlib.game.item.weapon
{
    public class ItemStaff : ItemWeapon
    {
        public ItemStaff(string name, int damage, int cooldown) : base(name, Classes.magician, damage, cooldown)
        {
            this.renderRoll = 65;
        }

        public override void Use(EntityPlayer player)
        {
            base.Use(player);
            player.GetWorld().SpawnEntity(new EntityProjectileStaff(player.GetWorld(), player));
        }
    }
}
