using ancient.game.world;
using ancientlib.game.item.projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.projectile
{
    public class EntityArrow : EntityProjectile
    {
        public EntityArrow(World world) : base(world)
        { }

        public EntityArrow(World world, EntityLiving shooter, ItemArrow arrow) : base(world, shooter, arrow)
        { }
    }
}
