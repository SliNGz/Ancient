using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.world;
using ancient.game.entity;
using ancient.game.entity.player;

namespace ancientlib.game.entity.ai
{
    class EntityAILookAtPlayer : EntityAI
    {
        private EntityLiving entity;
        private World world;

        private float distance;

        private EntityPlayer player;
        private double lookTime;

        public EntityAILookAtPlayer(EntityLiving entity, int priority, float distance) : base(priority)
        {
            this.entity = entity;
            this.world = entity.GetWorld();
            this.distance = distance;
        }

        public override void Execute()
        {
            this.lookTime = world.rand.Next(3, 7);
        }

        public override bool ShouldExecute()
        {
            if (world.rand.Next(20) == 0)
                return false;

            player = entity.GetNearestPlayer(distance);
            return player != null;
        }

        public override bool ShouldUpdate()
        {
            return /* this.lookTime > 0 */ player != null && Vector3.Distance(entity.GetEyePosition(), player.GetEyePosition()) <= this.distance;
        }

        public override void Stop()
        { }

        public override void Update(GameTime gameTime)
        {
            this.lookTime -= gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 eyePosition = player.GetEyePosition();
            entity.SetLookAtDestination(eyePosition.X, eyePosition.Y, eyePosition.Z);
        }
    }
}
