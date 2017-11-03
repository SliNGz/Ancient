using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity
{
    public abstract class EntityBase
    {
        protected World world;

        protected Vector3 position;
        protected Vector3 velocity;
        protected Vector3 acceleration;

        protected Vector3 rotation;
        protected Vector3 rotationVelocity;

        protected Vector3 dimensions;
        protected BoundingBox boundingBox;

        protected int ticksExisted;
        protected int lifeSpan;

        public EntityBase(World world)
        { }
    }
}
