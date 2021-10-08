using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancientlib.game.skill;
using ancient.game.entity.player;
using ancient.game.world.block;
using ancientlib.game.entity.model;

namespace ancientlib.game.entity.skill
{
    public class EntitySkill : Entity
    {
        private Skill skill;
        private EntityPlayer player;

        public EntitySkill(World world, Skill skill) : base(world)
        {
            this.skill = skill;
            this.player = skill.GetPlayer();
            this.yaw = player.GetYaw();
            this.lifeSpan = skill.GetEntityLifeSpan();
            this.gravity = 0;
            this.interactWithBlocks = false;
            this.interactWithEntities = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.colorMultiply.A = (byte)(255 - (ticksExisted / (float)lifeSpan) * 255);
        }

        public Skill GetSkill()
        {
            return this.skill;
        }

        protected override bool CanCollideWithBlockBoundingBox(Block block)
        {
            return false;
        }

        protected override bool CanCollideWithEntityBoundingBox(Entity entity)
        {
            return false;
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.1F, 0.1F, 0.1F);
        }

        public override EntityModelCollection GetModelCollection()
        {
            if (skill != null)
                return skill.GetModelCollection();

            return null;
        }

        public override string GetEntityName()
        {
            return "skill";
        }
    }
}
