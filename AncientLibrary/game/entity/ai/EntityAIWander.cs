using ancient.game.entity;
using ancient.game.utils;
using ancient.game.world;
using ancientlib.game.utils;
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
        private float wanderTime;
        private bool hasPath;
        private int x;
        private int y;
        private int z;

        public EntityAIWander(EntityLiving entity, int priority) : base(priority)
        {
            this.entity = entity;
            this.world = entity.GetWorld();
            this.hasPath = false;
        }

        public override void Execute()
        {
            this.x = (int)entity.GetX() + world.rand.Next(4) * world.rand.NextSign();
            this.z = (int)entity.GetZ() + world.rand.Next(4) * world.rand.NextSign();
            this.y = world.GetSurfaceHeight(x, z) + (entity is EntityFlying ? world.rand.Next(0, 4) : 0);

            if (y != World.MAX_HEIGHT)
            {
                this.entity.GetPathFinder().SetPath(x, y, z);
                hasPath = true;

                this.wanderTime = world.rand.Next(7, 11);
            }
        }

        public override bool ShouldExecute()
        {
            if (entity is EntityPet && ((EntityPet)entity).HasOwner())
                return false;

            bool onGround = entity.OnGround();

            if (entity is EntityFlying)
                onGround = true;

            return onGround && !hasPath && world.rand.Next(2) == 0;
        }

        public override bool ShouldUpdate()
        {
            return !HasReachedDestination() && wanderTime > 0;
        }

        public override void Stop()
        {
            entity.GetPathFinder().ClearPath();
            hasPath = false;
        }

        public override void Update(GameTime gameTime)
        {
            wanderTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private bool HasReachedDestination()
        {
            return x >= Math.Floor(entity.GetBoundingBox().GetCenter().X) && x <= Math.Ceiling(entity.GetBoundingBox().GetCenter().X) &&
                   z >= Math.Floor(entity.GetBoundingBox().GetCenter().Z) && z <= Math.Ceiling(entity.GetBoundingBox().GetCenter().Z) &&
                   y >= Math.Floor(entity.GetBoundingBox().GetCenter().Y) && y <= Math.Ceiling(entity.GetBoundingBox().GetCenter().Y);
        }
    }
}
