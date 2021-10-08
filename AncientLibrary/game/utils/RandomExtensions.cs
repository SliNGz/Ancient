using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public static class RandomExtensions
    {
        public static bool NextBool(this Random rand)
        {
            return rand.Next(2) == 0 ? true : false;
        }

        public static int NextSign(this Random rand)
        {
            return rand.Next(2) == 0 ? -1 : 1;
        }

        public static float Next(this Random rand, float min, float max)
        {
            return (float)(min + rand.NextDouble() * (max - min));
        }
    }
}
