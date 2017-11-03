using ancient.game.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.entity;
using ancientlib.game.world.chunk;
using ancient.game.entity.player;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet;
using ancientlib.game.world.entity;
using ancientlib.game.entity;
using ancientlib.game.network;
using ancientlib.game.network.packet.common.status;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.entity.world;
using ancientlib.game.init;
using ancientlib.game.network.packet.server.world;

namespace ancientlib.game.world
{
    public class WorldServer : World
    {
        private NetEntityManager netEntityManager;

        public WorldServer(int seed) : base(seed)
        {
            this.chunkLoader = new ChunkLoader(this);
            this.netEntityManager = new NetEntityManager();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            netEntityManager.Update();
        }

        public void BroadcastPacket(Packet packet)
        {
            for (int i = 0; i < players.Count; i++)
                ((EntityPlayerOnline)players[i]).netConnection.SendPacket(packet);
        }

        public void KickPlayer(EntityPlayer player, string message = "You were kicked from the server")
        {
            EntityPlayerOnline playerOnline = (EntityPlayerOnline)player;

            playerOnline.netConnection.SendPacket(new PacketDisconnect(message));
            DespawnEntity(player);
        }

        public override void SpawnEntity(Entity entity)
        {
            base.SpawnEntity(entity);
            netEntityManager.AddNetEntity(entity);
        }

        public override void DespawnEntity(Entity entity)
        {
            base.DespawnEntity(entity);
            netEntityManager.RemoveNetEntity(entity);
        }

        public override void OnDespawnEntity(Entity entity)
        {
            base.OnDespawnEntity(entity);

            if (entity is EntityPlayerOnline)
                NetServer.instance.RemoveNetConnection(((EntityPlayerOnline)entity).netConnection);

            PacketDespawnEntity despawnPacket = new PacketDespawnEntity(entity);
            for (int i = 0; i < players.Count; i++)
            {
                if (entity != players[i])
                    ((EntityPlayerOnline)players[i]).netConnection.SendPacket(despawnPacket);
            }
        }

        public NetEntityManager GetNetEntityManager()
        {
            return this.netEntityManager;
        }

        protected override void OnWeatherChange()
        {
            base.OnWeatherChange();
            BroadcastPacket(new PacketToggleRain(isRaining));
        }
    }
}
