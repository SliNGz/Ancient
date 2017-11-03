using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.ai
{
    class EntityAISeemIdle : EntityAI
    {
        private World world;
        private double idleTime;

        public EntityAISeemIdle(EntityLiving entity, int priority) : base(priority)
        {
            this.world = entity.GetWorld();
        }

        public override void Execute()
        {
            this.idleTime = world.rand.Next(4, 9);
        }

        public override bool ShouldExecute()
        {
            return true;
        }

        public override bool ShouldUpdate()
        {
            return this.idleTime > 0;
        }

        public override void Stop()
        { }

        public override void Update(GameTime gameTime)
        {
            idleTime -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
