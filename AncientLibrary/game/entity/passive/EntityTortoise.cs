using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.ai;
using Microsoft.Xna.Framework;
using ancientlib.game.init;

namespace ancientlib.game.entity
{
    public class EntityTortoise : EntityMount
    {
        private static EntityModelState DEFAULT = new EntityModelState("tortoise", 1.4f, 1.1f, 1.4f);
        private static EntityModelState SLEEPING = new EntityModelState("tortoise_sleeping", 1.4f, 0.9F, 1.4f);

        public EntityTortoise(World world) : base(world)
        {
            this.maxHealth = 100000;
            this.health = maxHealth;
            this.runningSpeed = 1.25F;

            this.expReward = 3;

            this.aiManager.AddTask(new EntityAIMount(this, 0));
            this.aiManager.AddTask(new EntityAIFollowOwner(this, 1, 4, 32));
            this.aiManager.AddTask(new EntityAIRunAround(this, 2));
            this.aiManager.AddTask(new EntityAILookAtPlayer(this, 3, 12));
            this.aiManager.AddTask(new EntityAIWander(this, 4));
            this.aiManager.AddTask(new EntityAISeemIdle(this, 4));

            this.food = Items.carrot;
            SetModelState(DEFAULT);
        }

        protected override void DropItems()
        {
            base.DropItems();
            DropItem(Items.berkin, world.rand.Next(1, 3), 95);
        }

        public override Vector3 GetMountOffset()
        {
            return new Vector3(0, owner.GetHeight(), 0);
        }

        public override float GetBaseSpeed()
        {
            return 3.75F;
        }

        public override float GetBaseJumpSpeed()
        {
            return 5.0F;
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.05F, 0.05F, 0.05F);
        }

        protected override EntityModelState GetDefaultModelState()
        {
            return DEFAULT;
        }
    }
}
