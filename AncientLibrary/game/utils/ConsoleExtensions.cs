using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class ConsoleExtensions
    {
        public static ConsoleColor DEFAULT_COLOR = ConsoleColor.White;

        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = DEFAULT_COLOR;
        }
    }
}
