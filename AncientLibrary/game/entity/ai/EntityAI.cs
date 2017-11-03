using ancient.game.entity;
using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.ai
{
    public abstract class EntityAI
    {
        protected int priority;

        public EntityAI(int priority)
        {
            this.priority = priority;
        }

        public abstract void Execute();

        public abstract bool ShouldExecute();

        public abstract bool ShouldUpdate();

        public abstract void Stop();

        public abstract void Update(GameTime gameTime);

        public int GetPriority()
        {
            return this.priority;
        }
    }
}
