using ancientlib.game.classes.bowman;
using ancientlib.game.classes.magician;
using ancientlib.game.classes.thief;
using ancientlib.game.classes.warrior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.classes
{
    class Classes
    {
        private static Dictionary<int, Class> classes = new Dictionary<int, Class>();

        public static ClassWarrior warrior = new ClassWarrior();
        public static ClassMagician magician = new ClassMagician();
        public static ClassBowman bowman = new ClassBowman();
        public static ClassThief thief = new ClassThief();

        public static void Initialize()
        {
            InitializeClass(0, warrior);
            InitializeClass(1, magician);
            InitializeClass(2, bowman);
            InitializeClass(3, thief);
        }

        private static void InitializeClass(int id, Class _class)
        {
            classes.Add(id, _class);
        }

        public static Class GetClassFromID(int id)
        {
            Class _class = null;
            classes.TryGetValue(id, out _class);

            return _class;
        }

        public static int GetIDFromClass(Class _class)
        {
            return classes.First(x => x.Value == _class).Key;
        }
    }
}
