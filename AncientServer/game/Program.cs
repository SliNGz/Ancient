using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Diagnostics;

namespace ancientserver.game
{
    class Program
    {
        public static AncientServer game;
        public static int tickRate = 128;

        static void Main(string[] args)
        {
            Console.Title = "Ancient Server";
            game = new AncientServer();
            game.Initalize();

            GameTime gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1.0 / tickRate));

            while (true)
            {
                game.Update(gameTime);

                Thread.Sleep(gameTime.ElapsedGameTime);
                gameTime.TotalGameTime += gameTime.ElapsedGameTime;
            }
        }
    }
}
