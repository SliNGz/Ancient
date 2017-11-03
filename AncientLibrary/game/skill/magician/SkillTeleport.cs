using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using Microsoft.Xna.Framework;

namespace ancientlib.game.skill.magician
{
    public class SkillTeleport : Skill
    {
        private float distance;

        public SkillTeleport(EntityPlayer player) : base(player)
        {
            this.name = "Teleport";
            this.manaConsume = 0;
            this.distance = 5;
            this.maxLevel = 10;
        }

        public override void Activate()
        {
            base.Activate();

            Vector3? blockPos = player.GetTargetedBlockPosition(distance);

            if (blockPos.HasValue)
            {
                Vector3 teleport = blockPos.Value;
                player.SetPosition(teleport + new Vector3(0, player.GetHeight() + 1.05F, 0));
            }
        }

        public override Vector3 GetModelOffset()
        {
            return Vector3.Zero;
        }

        public override string GetSoundName()
        {
            return null;
        }

        public override void OnLevelUp()
        {
            this.distance *= 1.2F;
            this.manaConsume = (int)Math.Round(this.manaConsume * 0.85F);
        }
    }
}
