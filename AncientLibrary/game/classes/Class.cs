using ancient.game.entity.player;
using ancientlib.game.classes.magician;
using ancientlib.game.item;
using ancientlib.game.skill;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.classes
{
    public abstract class Class
    {
        protected string name;
        protected Type[] skills;

        public Class(string name)
        {
            this.name = name;
            this.skills = new Type[4];
        }

        public string GetName()
        {
            return this.name;
        }

        public Type[] GetSkills()
        {
            return this.skills;
        }

        public int GetDamage(EntityPlayer player)
        {
            return GetAttackInfo(player).GetDamage();
        }

        public AttackInfo GetAttackInfo(EntityPlayer player)
        {
            ItemStack hand = player.GetItemInHand();

            if (hand == null || (hand != null && !(hand.GetItem() is ItemWeapon)))
                return new AttackInfo(player, 0);

            ItemWeapon weapon = (ItemWeapon)hand.GetItem();
            int damage = weapon.GetDamage();

            if (this is ClassMagician)
                damage += (int)(player.GetWsd() * 2.5F);
            else
                damage += (int)(player.GetStr() * 2.5F);

            Random rand = player.GetWorld().rand;

            damage = AttackUtils.GetRandomDamage(rand, damage, 0.07F);

            bool isCritical = false;

            if (AttackUtils.ShouldApplyCriticalHit(rand, player.GetCriticalHitChance()))
            {
                damage = AttackUtils.GetCriticalHitDamage(rand, damage, player.GetCriticalHitPercentage());
                isCritical = true;
            }

            return new AttackInfo(player, damage).SetCritical(isCritical);
        }
    }
}
