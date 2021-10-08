using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.model;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.ai;
using ancientlib.game.init;

namespace ancientlib.game.entity.passive
{
    class EntityBee : EntityFlying
    {
        public EntityBee(World world) : base(world)
        {
           // this.aiManager.AddTask(new EntityAIFlyToBlock(this, 0, Blocks.flowers));
            this.aiManager.AddTask(new EntityAIWander(this, 0));
         //   this.aiManager.AddTask(new EntityAISeemIdle(this, 0));
        }

        public override string GetEntityName()
        {
            return "bee";
        }

        public override EntityModelCollection GetModelCollection()
        {
            return EntityModelCollection.bee;
        }

        public override Vector3 GetModelScale()
        {
            return Vector3.One / 16F;
        }
    }
}
