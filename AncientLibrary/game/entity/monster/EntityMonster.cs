using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;

namespace ancientlib.game.entity.monster
{
    public abstract class EntityMonster : EntityLiving
    {
        public EntityMonster(World world) : base(world)
        { }

        public override bool IsHostile()
        {
            return true;
        }
    }
}
