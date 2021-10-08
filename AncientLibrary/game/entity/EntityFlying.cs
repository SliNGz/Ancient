using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.model;
using Microsoft.Xna.Framework;

namespace ancientlib.game.entity
{
    public abstract class EntityFlying : EntityLiving
    {
        public EntityFlying(World world) : base(world)
        {
            this.gravity = 0;
        }
    }
}
