using ancientlib.game.constants;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.stats
{
    public class StatTable
    {
        private static int[] exp;
        private static int[] health;

        public static void Initialize()
        {
            exp = new int[GameConstants.MAX_LEVEL + 1];

            //Each cell in the exp array is the needed exp to level up to the next level.
            for (int level = 1; level <= GameConstants.MAX_LEVEL; level++)
                exp[level] = (int)(exp[level - 1] + Math.Round(82.5 * Math.Pow(2, level / 12.5)));

            health = new int[GameConstants.MAX_LEVEL + 1];

            for (int level = 1; level <= GameConstants.MAX_LEVEL; level++)
                health[level] = (int)MathHelper.Lerp(250, 50000, (level - 1) / ((float)GameConstants.MAX_LEVEL - 1));
        }

        public static int GetExpToNextLevel(int level)
        {
            if (level < GameConstants.MAX_LEVEL)
                return exp[level];

            return int.MaxValue;
        }

        public static int GetMaxHealth(int level)
        {
            return health[level];
        }
    }
}
