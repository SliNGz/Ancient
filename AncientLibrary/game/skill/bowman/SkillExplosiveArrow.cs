using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.projectile;
using ancientlib.game.init;
using ancientlib.game.entity;

namespace ancientlib.game.skill.bowman
{
    public class SkillExplosiveArrow : Skill
    {
        private float damageMultiplier;
        private int explosionSize;

        public SkillExplosiveArrow(EntityPlayer player) : base(player)
        {
            SetLevel(20);
        }

        public override void Activate()
        {
            base.Activate();
            EntityExplosiveArrow explosiveArrow = new EntityExplosiveArrow(player.GetWorld(), player, Items.explosiveArrowSkill);
            explosiveArrow.SetDamage((int)(explosiveArrow.GetDamage() * damageMultiplier));
            explosiveArrow.SetExplosionSize(explosionSize);
            player.GetWorld().SpawnEntity(explosiveArrow);
        }

        public override string GetActivationSound(Random rand)
        {
            return "shoot_arrow_" + rand.Next(4);
        }

        public override int GetEntityLifeSpan()
        {
            return 0;
        }

        public override EntityModelState GetModelState()
        {
            return null;
        }

        public override int GetMaxLevel()
        {
            return 20;
        }

        protected override void OnLevelChanged()
        {
            base.OnLevelChanged();
            UpdateDamageMultiplier();
            UpdateExplosionSize();
        }

        public override string GetName()
        {
            return "Explosive Arrow";
        }

        protected override void UpdateCooldown()
        {
            int initialCooldown = 20;
            int level = this.level == 1 ? 0 : this.level;
            this.cooldown = 0;// 128 * (int)Math.Round(initialCooldown - level * 0.7);
        }

        protected override void UpdateManaConsumption()
        {
            int initialConsumption = 65;
            this.manaConsumption = initialConsumption - (level * 2);
        }

        private void UpdateDamageMultiplier()
        {
            float initialDamageMultiplier = 60;
            int level = this.level == 1 ? 0 : this.level;
            this.damageMultiplier = (initialDamageMultiplier + level * 8) / 100F;
        }

        private void UpdateExplosionSize()
        {
            int initialSize = 1;
            int level = this.level == 1 ? 0 : this.level;
            this.explosionSize = (int)(initialSize + level * 0.2);
        }
    }
}
