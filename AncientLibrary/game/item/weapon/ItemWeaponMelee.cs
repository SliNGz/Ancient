using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.classes;
using ancient.game.entity.player;
using ancientlib.game.entity;
using ancientlib.game.utils;
using ancient.game.entity;

namespace ancientlib.game.item.weapon
{
    public abstract class ItemWeaponMelee : ItemWeapon
    {
        protected float range;

        public ItemWeaponMelee(string name, Class _class, int damage, int cooldown, float range) : base(name, _class, damage, cooldown)
        {
            this.range = range;
        }

        public override void Use(EntityPlayer player, ItemStack itemStack)
        {
            base.Use(player, itemStack);
            Entity entity = player.GetLookAtEntityFromList(player.GetWorld().entityList.OfType<EntityLiving>(), range);

            if(entity is EntityLiving)
                ((EntityLiving)entity).Damage(player.GetAttackInfo());
        }

        public float GetRange()
        {
            return this.range;
        }
    }
}
