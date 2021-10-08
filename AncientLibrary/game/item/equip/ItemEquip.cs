using ancientlib.game.item.statbased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.classes;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;

namespace ancientlib.game.item.equip
{
    public abstract class ItemEquip : ItemStatDependent
    {
        protected int strAdd;
        protected int wsdAdd;
        protected int dexAdd;
        protected int lukAdd;

        protected Vector3 modelOffset;
        protected Vector3 modelScale;

        public ItemEquip(string name, Class _class, int level, int str, int wsd, int dex, int luk, int strAdd, int wsdAdd, int dexAdd, int lukAdd) :
            base(name, _class, level, str, wsd, dex, luk)
        {
            this.maxItemStack = 1;
            this.strAdd = strAdd;
            this.wsdAdd = wsdAdd;
            this.dexAdd = dexAdd;
            this.lukAdd = lukAdd;

            this.modelOffset = Vector3.Zero;
            this.modelScale = Vector3.One;
        }

        public override void Use(EntityPlayer player, ItemStack itemStack)
        {
            base.Use(player, itemStack);
            player.usingItemInHand = false;
        }

        public int GetStrAddition()
        {
            return this.strAdd;
        }

        public int GetWsdAddition()
        {
            return this.wsdAdd;
        }

        public int GetDexAddition()
        {
            return this.dexAdd;
        }

        public int GetLukAddition()
        {
            return this.lukAdd;
        }

        public Vector3 GetModelOffset()
        {
            return this.modelOffset;
        }

        public ItemEquip SetModelOffset(Vector3 modelOffset)
        {
            this.modelOffset = modelOffset;
            return this;
        }

        public Vector3 GetModelScale()
        {
            return this.modelScale;
        }

        public ItemEquip SetModelScale(Vector3 modelScale)
        {
            this.modelScale = modelScale;
            return this;
        }
    }
}
