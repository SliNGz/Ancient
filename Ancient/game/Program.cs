using System;

namespace ancient.game
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Ancient ancient = new Ancient();
            ancient.Run();
        }
    }
#endif
}
