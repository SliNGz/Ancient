using ancient.game.entity;
using ancient.game.entity.player;
using ancientlib.game.entity;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.network.packet.server.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.entity
{
    public class NetEntity
    {
        public static int POSITION_SEND_RATE = 8;
        public static int ROTATION_SEND_RATE = 8;

        protected WorldServer world;
        protected Entity entity;

        protected int posTicks;
        protected double lastX;
        protected double lastY;
        protected double lastZ;

        protected int rotTicks;
        protected float lastYaw;

        public NetEntity(Entity entity)
        {
            this.world = (WorldServer)entity.GetWorld();
            this.entity = entity;
            BroadcastSpawn();
        }

        public virtual void Update()
        {
            UpdatePosition();
            UpdateRotation();
        }

        private void BroadcastSpawn()
        {
            PacketSpawnEntity spawnPacket = new PacketSpawnEntity(entity);

            for (int i = 0; i < world.players.Count; i++)
            {
                EntityPlayerOnline player = (EntityPlayerOnline)world.players[i];

                if (player != entity)
                    player.netConnection.SendPacket(spawnPacket);
            }
        }

        private void UpdatePosition()
        {
            if (!entity.ShouldSendPosition())
                return;

            posTicks++;

            if (posTicks == POSITION_SEND_RATE)
            {
                if (ShouldSendPosition())
                {
                    PacketEntityPosition positionPacket = new PacketEntityPosition(entity);

                    for (int i = 0; i < world.players.Count; i++)
                    {
                        EntityPlayerOnline player = (EntityPlayerOnline)world.players[i];

                        if (player != entity)
                            player.netConnection.SendPacket(positionPacket);
                    }

                    this.lastX = entity.GetX();
                    this.lastY = entity.GetY();
                    this.lastZ = entity.GetZ();
                }

                posTicks = 0;
            }
        }

        private bool ShouldSendPosition()
        {
            return entity.GetX() != this.lastX || entity.GetY() != this.lastY || entity.GetZ() != this.lastZ;
        }

        private void UpdateRotation()
        {
            if (!entity.ShouldSendRotation())
                return;

            rotTicks++;

            if (rotTicks == ROTATION_SEND_RATE)
            {
                if (ShouldSendRotation())
                {
                    PacketEntityRotation rotationPacket = new PacketEntityRotation(entity);

                    for (int i = 0; i < world.players.Count; i++)
                    {
                        EntityPlayerOnline player = (EntityPlayerOnline)world.players[i];

                        if (player != entity)
                            player.netConnection.SendPacket(rotationPacket);
                    }

                    this.lastYaw = entity.GetYaw();
                }

                rotTicks = 0;
            }
        }

        private bool ShouldSendRotation()
        {
            return entity.GetYaw() != this.lastYaw;
        }

        public Entity GetEntity()
        {
            return this.entity;
        }
    }
}
