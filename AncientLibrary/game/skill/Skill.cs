using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.classes;
using ancientlib.game.entity;
using ancientlib.game.entity.skill;
using ancientlib.game.item;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.skill
{
    /*  public abstract class Skill
      {
          protected EntityPlayer player;

          protected int level;
          protected int maxLevel;

          protected int manaConsume;

          protected int lifeSpan;

          protected int cooldown;
          protected int ticksRemain;

          public Skill(EntityPlayer player)
          {
              this.player = player;
              this.level = 1;
              this.maxLevel = 1;
              this.manaConsume = 0;
              this.cooldown = 0;
              this.lifeSpan = 0;
          }

          public virtual void Activate()
          {
              World world = player.GetWorld();
              world.PlaySound(GetSoundName());
              player.AddMana(-manaConsume);

              this.ticksRemain = cooldown;
          }

          public virtual bool CanActivate()
          {
              return player.GetMana() >= manaConsume && ticksRemain <= 0;
          }

          public abstract string GetName();

          public abstract string GetSoundName();

          public abstract void OnLevelUp();

          public virtual void Update()
          {
              ticksRemain--;
          }

          public int GetLifeSpan()
          {
              return this.lifeSpan;
          }

          public string GetModelName()
          {
              string modelName = GetName();
              modelName = modelName.ToLower();
              modelName = modelName.Replace(' ', '_');
              return modelName;
          }

          public void LevelUp()
          {
              this.level++;
              OnLevelUp();
          }
      }*/

    public abstract class Skill
    {
        protected EntityPlayer player;

        protected int level;

        protected int cooldown;
        protected int ticksRemain;

        protected int manaConsumption;

        public Skill(EntityPlayer player)
        {
            this.player = player;
            this.level = 1;
            this.cooldown = 0;
            this.ticksRemain = 0;
            this.manaConsumption = 0;
            OnLevelChanged();
        }

        public virtual void Update()
        {
            if (ticksRemain > 0)
                ticksRemain--;
        }

        public virtual void Activate()
        {
            player.GetWorld().PlaySound(GetActivationSound(player.GetWorld().rand));
            player.AddMana(-manaConsumption);

            this.ticksRemain = cooldown;
        }

        public virtual bool CanActivate()
        {
            ItemStack hand = player.GetItemInHand();

            bool usingClassWeapon = false;

            if (hand != null && hand.GetItem() is ItemWeapon)
            {
                ItemWeapon weapon = (ItemWeapon)hand.GetItem();

                if (weapon.GetClass() == player.GetClass())
                    usingClassWeapon = true;
            }

            return player.IsAlive() && usingClassWeapon && player.GetMana() >= manaConsumption && ticksRemain <= 0;
        }

        public EntityPlayer GetPlayer()
        {
            return this.player;
        }

        public int GetLevel()
        {
            return this.level;
        }

        public void SetLevel(int level)
        {
            this.level = MathHelper.Clamp(level, 0, GetMaxLevel());
            OnLevelChanged();
        }

        public void LevelUp()
        {
            SetLevel(this.level + 1);
        }

        protected virtual void OnLevelChanged()
        {
            UpdateCooldown();
            UpdateManaConsumption();
        }

        public abstract string GetName();

        public abstract int GetMaxLevel();

        protected abstract void UpdateCooldown();

        protected abstract void UpdateManaConsumption();

        public abstract int GetEntityLifeSpan();

        public abstract EntityModelState GetModelState();

        public abstract string GetActivationSound(Random rand);
    }
}
