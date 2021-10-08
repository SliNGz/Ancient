using ancientlib.game.skill.bowman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.classes.bowman
{
    public class ClassBowman : Class
    {
        public ClassBowman() : base("Bowman")
        {
            this.skills = new Type[1];
            this.skills[0] = typeof(SkillExplosiveArrow);
        }
    }
}
