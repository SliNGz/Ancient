using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.ai;
using Microsoft.Xna.Framework;
using ancientlib.game.utils;
using ancientlib.game.init;
using ancientlib.game.entity.world;
using ancientlib.game.item;

namespace ancientlib.game.entity.passive
{
    public class EntitySlime : EntityPet
    {
        public EntitySlime(World world) : base(world)
        {
            this.maxHealth = 20;
            this.health = maxHealth;

            SetDimensions(0.85F, 0.85F, 0.85F);

            this.expReward = 2;

            this.aiManager.AddTask(new EntityAIFollowOwner(this, 0, 2));
            this.aiManager.AddTask(new EntityAIRunAround(this, 1));
            this.aiManager.AddTask(new EntityAILookAtPlayer(this, 2, 12));
            this.aiManager.AddTask(new EntityAIWander(this, 3));
            this.aiManager.AddTask(new EntityAISeemIdle(this, 3));

            this.colorMultiply = Utils.HSVToRGB(100, 1, 1);

            this.food = Items.blueberries;
        }

        protected override void DropItems()
        {
            base.DropItems();
            DropItem(Items.berkin, world.rand.Next(1, 3), 95);
            DropItem(Items.blueberries, world.rand.Next(1, 3), 10);
        }

        public override string GetModelName()
        {
            return "slime";
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.05F, 0.05F, 0.05F);
        }

        public override double GetBaseSpeed()
        {
            return 3.5;
        }

        public override double GetBaseJumpSpeed()
        {
            return 7.0;
        }

        public override bool IsHostile()
        {
            return true;
        }
    }
}
