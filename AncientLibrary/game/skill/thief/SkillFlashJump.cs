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

namespace ancientlib.game.skill.thief
{
    public class SkillFlashJump : Skill
    {
        private float distance;

        public SkillFlashJump(EntityPlayer player) : base(player)
        {
            this.name = "Flash Jump";
            this.manaConsume = 70;
            this.distance = 10;
            this.maxLevel = 10;
            this.cooldown = Utils.TicksInSecond * 10;
            this.lifeSpan = 384;
        }

        public override void Activate()
        {
            base.Activate();
            player.GetWorld().SpawnEntity(new EntitySkill(player.GetWorld(), player, this));

            Vector3 jumpVector = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), MathHelper.ToRadians(45), 0));
            jumpVector.Normalize();

            player.AddVelocity(jumpVector * new Vector3(distance, distance / 5F, distance));
        }

        public override Vector3 GetModelOffset()
        {
            return new Vector3(0, 3, 0);
        }

        public override string GetSoundName()
        {
            return "flash_jump";
        }

        public override void OnLevelUp()
        {
            this.manaConsume = (int)(manaConsume * 0.95F);
            this.distance = MathHelper.Lerp(10, 50, (level - 1) / (float)(maxLevel - 1));
            this.cooldown = (int)(cooldown * 0.9F);
        }
    }
}
