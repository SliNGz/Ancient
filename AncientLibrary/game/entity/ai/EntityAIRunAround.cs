using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.world;

namespace ancientlib.game.entity.ai
{
    class EntityAIRunAround : EntityAIWander
    {
        public EntityAIRunAround(EntityLiving entity, int priority) : base(entity, priority)
        {
            this.entity = entity;
        }

        public override bool ShouldExecute()
        {
            if (entity is EntityPet && ((EntityPet)entity).HasOwner())
                return false;

            return entity.OnGround() && entity.IsAttacked();
        }
    }
}
