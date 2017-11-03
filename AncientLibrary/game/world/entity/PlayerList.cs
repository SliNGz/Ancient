using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.entity.player;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.entity
{
    public class PlayerList : List<EntityPlayer>
    {
        private Queue<EntityPlayer> unloadedPlayers;

        private World world;
        private int timeoutTicks;

        public PlayerList(World world)
        {
            this.unloadedPlayers = new Queue<EntityPlayer>();

            this.world = world;
        }

        public void Update()
        {
            KickTimedoutPlayers();

            while (unloadedPlayers.Count > 0)
                Remove(unloadedPlayers.Dequeue());
        }

        private void KickTimedoutPlayers()
        {
            if (!(world is WorldServer))
                return;

            WorldServer worldServer = (WorldServer)world;

            timeoutTicks++;

            if (timeoutTicks == 640)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is EntityPlayerOnline)
                    {
                        EntityPlayerOnline player = (EntityPlayerOnline)this[i];

                        if (!player.netConnection.IsTimedout())
                        {
                            worldServer.KickPlayer(player);
                            ConsoleExtensions.WriteLine(ConsoleColor.Yellow, player.GetName() + " has timed out.");
                        }
                    }
                }

                timeoutTicks = 0;
            }
        }

        public void RemovePlayer(EntityPlayer player)
        {
            unloadedPlayers.Enqueue(player);
        }
    }
}
