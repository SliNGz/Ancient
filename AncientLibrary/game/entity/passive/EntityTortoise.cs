using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.ai;
using Microsoft.Xna.Framework;
using ancientlib.game.init;
using ancientlib.game.entity.model;

namespace ancientlib.game.entity
{
    public class EntityTortoise : EntityMount
    {
        public EntityTortoise(World world) : base(world)
        {
            this.maxHealth = 100000;
            this.health = maxHealth;
            this.runningSpeed = 1.25F;

            this.expReward = 300;

            this.aiManager.AddTask(new EntityAIMount(this, 0));
            this.aiManager.AddTask(new EntityAIFollowOwner(this, 1, 4, 32));
           // this.aiManager.AddTask(new EntityAIRunAround(this, 2));
            this.aiManager.AddTask(new EntityAILookAtPlayer(this, 3, 12));
          //  this.aiManager.AddTask(new EntityAIWander(this, 4));
            this.aiManager.AddTask(new EntityAISeemIdle(this, 4));

            this.food = Items.carrot;
        }

        protected override void DropItems()
        {
            base.DropItems();
            DropItem(Items.berkin, world.rand.Next(1, 3), 95);
        }

        public override Vector3 GetMountOffset()
        {
            float y = 0;

            if (animationTicks != 0)
            {
                if (animationIndex == 1)
                    y = -1;
                else if (animationIndex == 3)
                    y = 1;

                y *= GetModelScale().Y / 2;
            }

            return new Vector3(0, ridingEntity.GetHeight() + y, 0);
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

        public override EntityModelCollection GetModelCollection()
        {
            return EntityModelCollection.tortoise;
        }

        public override string GetEntityName()
        {
            return "tortoise";
        }
    }
}
