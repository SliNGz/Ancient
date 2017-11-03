using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.classes;
using Microsoft.Xna.Framework;

namespace ancientlib.game.item.weapon
{
    public class ItemDagger : ItemWeaponMelee
    {
        public ItemDagger(string name, int damage, int cooldown, float range) : base(name, Classes.thief, damage, cooldown, range)
        {
            this.renderRoll = -55;
        }
    }
}
