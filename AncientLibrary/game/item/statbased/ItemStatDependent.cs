using ancientlib.game.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.item.statbased
{
    public abstract class ItemStatDependent : Item
    {
        protected Class _class;
        protected int level;
        protected int str;
        protected int wsd;
        protected int dex;
        protected int luk;

        public ItemStatDependent(string name, Class _class, int level, int str, int wsd, int dex, int luk) : base(name)
        {
            this._class = _class;
            this.level = level;
            this.str = str;
            this.wsd = wsd;
            this.dex = dex;
            this.luk = luk;
        }

        public override bool CanUseItem(EntityPlayer player)
        {
            bool stats = player.GetLevel() >= this.level && player.GetStr() >= this.str && player.GetWsd() >= this.wsd && player.GetDex() >= this.dex && player.GetLuk() >= this.luk;

            if (!stats)
                return false;

            if (_class == null)
                return base.CanUseItem(player);

            bool isSameClass = player.GetClass().GetType().IsSubclassOf(_class.GetType()) || player.GetClass() == _class;

            if (isSameClass)
                return base.CanUseItem(player);

            return false;
        }

        public Class GetClass()
        {
            return this._class;
        }

        public ItemStatDependent SetClass(Class _class)
        {
            this._class = _class;
            return this;
        }

        public ItemStatDependent SetLevel(int level)
        {
            this.level = level;
            return this;
        }

        public ItemStatDependent SetStr(int str)
        {
            this.str = str;
            return this;
        }

        public ItemStatDependent SetWsd(int wsd)
        {
            this.wsd = wsd;
            return this;
        }

        public ItemStatDependent SetDex(int dex)
        {
            this.dex = dex;
            return this;
        }

        public ItemStatDependent SetLuk(int luk)
        {
            this.luk = luk;
            return this;
        }
    }
}
