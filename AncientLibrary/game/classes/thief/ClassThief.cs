using ancientlib.game.skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.item;
using ancientlib.game.skill.thief;

namespace ancientlib.game.classes.thief
{
    class ClassThief : Class
    {
        public ClassThief() : base("Thief")
        {
            this.skills = new Type[1];
            this.skills[0] = typeof(SkillFlashJump);
        }
    }
}
