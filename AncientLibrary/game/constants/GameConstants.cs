using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.constants
{
    public class GameConstants
    {
        public static readonly string GAME_NAME = "Ancient";
        public static readonly double GAME_VERSION = 0.01;

        //Stats
        public static readonly int MAX_LEVEL = 256;
        public static readonly int MAX_HEALTH = 100000;
        public static readonly int MAX_MANA = 100000;
        public static readonly int MAX_DAMAGE = int.MaxValue;

        //Inventory
        public static readonly int INV_MAX_SLOTS = 256;
        public static readonly int INV_MAX_LINE_SIZE = 16;
    }
}
