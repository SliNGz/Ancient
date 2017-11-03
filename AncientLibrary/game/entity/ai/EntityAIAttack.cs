using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;

namespace ancientlib.game.entity.ai
{
    class EntityAIAttack : EntityAI
    {
        private EntityLiving entity;
        private float distance;
        private EntityPlayer playerToAttack;

        public EntityAIAttack(EntityLiving entity, int priority) : base(priority)
        {
            this.entity = entity;
        }

        public override void Execute()
        {

        }

        public override bool ShouldExecute()
        {
            if (entity.IsHostile())
            {
                playerToAttack = entity.GetNearestPlayerWithinRange(distance);
                return playerToAttack != null;
            }

            return entity.IsAttacked();
        }

        public override bool ShouldUpdate()
        {
            return false;
        }

        public override void Stop()
        { }

        public override void Update(GameTime gameTime)
        { }
    }
}
