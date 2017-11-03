using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancientlib.game.init;
using ancient.game.world;
using ancientlib.game.entity;
using ancientlib.game.world;
using System.Threading;
using ancientlib.game.network;
using ancientserver.game.network.packet.handler;
using ancientlib.game.utils;

namespace ancientserver.game
{
    class AncientServer
    {
        public static AncientServer ancientServer;

        private TimeSpan deltaTime;
        private GameTime gameTime;

        private TimeSpan currentTime;
        private TimeSpan accumulator;

        public WorldServer world;
        public NetServer netServer;

        public AncientServer()
        {
            ancientServer = this;
        }

        public void Initalize()
        {
            Console.ForegroundColor = ConsoleExtensions.DEFAULT_COLOR;

            InitializeGame();

            this.deltaTime = TimeSpan.FromSeconds(1.0 / Program.tickRate);
            this.gameTime = new GameTime(TimeSpan.Zero, deltaTime);

            this.currentTime = DateTime.Now.TimeOfDay;

            this.world = new WorldServer(15050);
            this.netServer = new NetServer(15050);
        }

        public void Update(GameTime gameTime)
        {
            TimeSpan newTime = DateTime.Now.TimeOfDay;
            TimeSpan frameTime = newTime - currentTime;
            currentTime = newTime;

            accumulator += frameTime;

            while (accumulator >= deltaTime)
            {
                DoUpdate(gameTime);

                accumulator -= deltaTime;
                gameTime.TotalGameTime += deltaTime;
            }

            Thread.Sleep(deltaTime);
        }

        private void DoUpdate(GameTime gameTime)
        {
            world.Update(gameTime);
            netServer.Update();
        }

        private void InitializeGame()
        {
            Init.Initialize();
            ServerPacketHandlers.Initialize();
        }
    }
}
