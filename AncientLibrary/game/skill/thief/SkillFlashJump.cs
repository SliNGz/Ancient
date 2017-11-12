using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.skill;
using ancientlib.game.utils;
using ancientlib.game.entity;

namespace ancientlib.game.skill.thief
{
    public class SkillFlashJump : Skill
    {
        private static EntityModelState DEFAULT = new EntityModelState("flash_jump", 0, 0, 0);

        public SkillFlashJump(EntityPlayer player) : base(player)
        {
            SetLevel(10);
        }

        public override void Activate()
        {
            base.Activate();
            EntitySkill entity = new EntitySkill(player.GetWorld(), this);
            entity.SetPosition(player.GetPosition() + new Vector3(0, 3, 0));
            player.GetWorld().SpawnEntity(entity);

            Vector3 jumpVector = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), MathHelper.ToRadians(45), 0));
            jumpVector.Normalize();

            float distance = GetDistance();
            player.AddVelocity(jumpVector * new Vector3(distance, distance / 5F, distance));
        }

        public int GetDistance()
        {
            int initalDistance = 7;
            float distanceFactor = 1.15F;
            return (int)(initalDistance * Math.Pow(distanceFactor, level));
        }

        public override string GetActivationSound(Random rand)
        {
            return "flash_jump";
        }


        public override int GetEntityLifeSpan()
        {
            return 512;
        }

        public override EntityModelState GetModelState()
        {
            return DEFAULT;
        }

        public override int GetMaxLevel()
        {
            return 10;
        }

        public override string GetName()
        {
            return "Flash Jump";
        }

        protected override void UpdateCooldown()
        {
            int initialCooldown = 960;
            float cooldownFactor = 0.675F;
            this.cooldown = (int)(initialCooldown * Math.Pow(cooldownFactor, level));
            Console.WriteLine(cooldown);
        }

        protected override void UpdateManaConsumption()
        {
            int initialConsumption = 70;
            float consumptionFactor = 0.87F;
            this.manaConsumption = (int)(initialConsumption * Math.Pow(consumptionFactor, level));
        }
    }
}
