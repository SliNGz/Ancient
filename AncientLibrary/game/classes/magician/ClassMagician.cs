using ancient.game.entity.player;
using ancientlib.game.item;
using ancientlib.game.skill.magician;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.classes.magician
{
    class ClassMagician : Class
    {
        public ClassMagician() : base("Magician")
        {
            this.skills = new Type[1];
            this.skills[0] = typeof(SkillTeleport);
        }
    }
}
