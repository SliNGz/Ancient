using ancientlib.game.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item.projectile
{
    public class ItemArrow : ItemProjectile
    {
        public ItemArrow(string name, int damage, float speed, float width, float height, float length, float gravity) : base(name, Classes.bowman, damage, speed, width, height, length, gravity)
        { }

        public override Type GetWeaponType()
        {
            return typeof(ItemBow);
        }
    }
}
