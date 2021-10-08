using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.model;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.ai;
using ancientlib.game.utils;

namespace ancientlib.game.entity.monster
{
    class EntityNymu : EntityMonster
    {
        public EntityNymu(World world) : base(world)
        {
            this.aiManager.AddTask(new EntityAIAttack(this, 0, 8, 8, 2, 32));
            this.aiManager.AddTask(new EntityAIWander(this, 1));
            this.aiManager.AddTask(new EntityAISeemIdle(this, 1));
        }

        public override string GetEntityName()
        {
            return "nymu";
        }

        public override EntityModelCollection GetModelCollection()
        {
            return EntityModelCollection.nymu;
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(1 / 19F, 1 / 19F, 1 / 19F);
        }

        public override int GetDamage()
        {
            return AttackUtils.GetRandomDamage(world.rand, 120, 0.17F);
        }
    }
}
