using ancientlib.game.skill.magician;
using ancientlib.game.skill.thief;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.skill
{
    public class Skills
    {
        private static Dictionary<int, Type> skills = new Dictionary<int, Type>();

        private static void Initialize()
        {
            InitializeSkill(0, typeof(SkillTeleport));
            InitializeSkill(1, typeof(SkillFlashJump));
        }

        private static void InitializeSkill(int id, Type type)
        {
            skills.Add(id, type);
        }
    }
}
