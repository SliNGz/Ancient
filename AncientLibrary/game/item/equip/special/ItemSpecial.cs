using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;

namespace ancientlib.game.item.equip.special
{
    public abstract class ItemSpecial : ItemEquip
    {
        public ItemSpecial(string name, Class _class, int level, int str, int wsd, int dex, int luk, int strAdd, int wsdAdd, int dexAdd, int lukAdd) :
            base(name, _class, level, str, wsd, dex, luk, strAdd, wsdAdd, dexAdd, lukAdd)
        { }
    }
}
