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
    class EntityAIWander : EntityAI
    {
        protected EntityLiving entity;
        private World world;
        private double wanderTime;

        public EntityAIWander(EntityLiving entity, int priority) : base(priority)
        {
            this.entity = entity;
            this.world = entity.GetWorld();
        }

        public override void Execute()
        {
            int x = (int)entity.GetX() + world.rand.Next(-3, 4);
            int z = (int)entity.GetZ() + world.rand.Next(-3, 4);
            int y = world.GetSurfaceHeight(x, z) + 1;

            if (y != World.MAX_HEIGHT)
                this.entity.GetPathFinder().SetPath(x, y, z);

            this.wanderTime = world.rand.Next(7, 11);
        }

        public override bool ShouldExecute()
        {
            if (entity is EntityPet && ((EntityPet)entity).HasOwner())
                return false;

            return entity.OnGround() && !entity.GetPathFinder().HasPath() && world.rand.Next(2) == 0;
        }

        public override bool ShouldUpdate()
        {
            return entity.GetPathFinder().HasPath() && wanderTime > 0;
        }

        public override void Stop()
        {
            entity.GetPathFinder().ClearPath();
        }

        public override void Update(GameTime gameTime)
        {
            wanderTime -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
