using ancient.game.entity.player;
using ancientlib.game.classes;
using ancientlib.game.entity;
using ancientlib.game.item.weapon;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item
{
    public class ItemSword : ItemWeaponMelee
    {
        public ItemSword(string name, int damage, int cooldown, float range) : base(name, Classes.warrior, damage, cooldown, range)
        { }
    }
}
