using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class ThreadUtils
    {
        private static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();

        public static Thread CreateThread(string name, ThreadStart threadStart, bool isBackground)
        {
            Thread thread = new Thread(threadStart) { IsBackground = isBackground, Name = name };
            threads.Add(name, thread);
            return thread;
        }

        public static void RemoveThread(string name)
        {
            GetThreadFromName(name).Abort();
            threads.Remove(name);
        }

        public static Thread GetThreadFromName(string name)
        {
            Thread thread = null;
            threads.TryGetValue(name, out thread);

            return thread;
        }

        public static void Clear()
        {
            foreach (Thread thread in threads.Values)
                thread.Abort();

            threads.Clear();
        }
    }
}
