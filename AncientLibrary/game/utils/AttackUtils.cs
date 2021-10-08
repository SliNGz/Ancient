using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class AttackUtils
    {
        public static int GetRandomDamage(Random rand, int damage, float deviation)
        {
            return damage + (int)(damage * (-deviation + rand.NextDouble() / (0.5 / deviation)));
        }

        public static int GetCriticalHitDamage(Random rand, int damage, float percentage)
        {
            return (int)(damage * (1 + rand.NextDouble() * percentage));
        }

        public static bool ShouldApplyCriticalHit(Random rand, float chance)
        {
            return rand.NextDouble() < chance;
        }
    }
}
