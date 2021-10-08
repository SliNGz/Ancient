using ancient.game.client.utils;
using ancientlib.game.user;
using System;

namespace ancient.game
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Ancient ancient = new Ancient();

            string username = "sling";
            string password = "admin";

            if (args.Length > 0)
            {
                username = args[0];
                password = args[1];
            }

            //ancient.user = new User(username, password);

            ancient.Window.Title = "Ancient - Username: " + username + " Password: " + password;

            ancient.Run();
        }
    }
#endif
}
