using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item.weapon
{
    public class ItemTwoHandedDagger : ItemDagger
    {
        public ItemTwoHandedDagger(string name, int damage, int cooldown, float range) : base(name, damage, cooldown, range)
        { }
    }
}
