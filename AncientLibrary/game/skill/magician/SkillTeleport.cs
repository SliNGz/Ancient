using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using Microsoft.Xna.Framework;
using ancientlib.game.entity;
using ancientlib.game.entity.model;

namespace ancientlib.game.skill.magician
{
    public class SkillTeleport : Skill
    {
        /*    public override void Activate()
            {
                base.Activate();

                Vector3? blockPos = player.GetTargetedBlockPosition(distance);

                if (blockPos.HasValue)
                {
                    Vector3 teleport = blockPos.Value;
                    player.SetPosition(teleport + new Vector3(0, player.GetHeight() + 1.05F, 0));
                }
            }*/
        public SkillTeleport(EntityPlayer player) : base(player)
        {
        }

        public override string GetActivationSound(Random rand)
        {
            return "shoot_arrow_" + rand.Next(4);
        }

        public override int GetEntityLifeSpan()
        {
            return 2048;
        }

        public override EntityModelCollection GetModelCollection()
        {
            return null;
        }

        public override int GetMaxLevel()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateCooldown()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateManaConsumption()
        {
            throw new NotImplementedException();
        }
    }
}
