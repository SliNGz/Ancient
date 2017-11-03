using ancient.game.client.world;
using ancient.game.entity.player;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.network.packet.common.player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.network
{
    public class NetPlay
    {
        public static int POSITION_SEND_RATE = 128;

        private WorldClient world;
        private EntityPlayer player;

        private Vector3 lastMovement;
        private bool wasJumping;
        private bool wasRunning;

        private float lastHeadYaw;
        private float lastHeadPitch;

        private int posTicks;

        public NetPlay(WorldClient world)
        {
            this.world = world;
            this.player = Ancient.ancient.player;
        }

        public void Update()
        {
            if (!world.IsRemote())
                return;

            SendPlayerRotation();
            SendPlayerInput();
            SendPlayerPosition();
        }

        private void SendPlayerInput()
        {
            if (ShouldSendInput())
            {
                NetClient.instance.SendPacket(new PacketPlayerInput(player));

                lastMovement = player.inputVector;
                wasJumping = player.IsJumping();
                wasRunning = player.IsRunning();
            }
        }

        private bool ShouldSendInput()
        {
            return player.inputVector.X != lastMovement.X || player.inputVector.Z != lastMovement.Z || player.IsJumping() != wasJumping || player.IsRunning() != wasRunning;
        }

        private void SendPlayerRotation()
        {
            if (ShouldSendRotation())
            {
                NetClient.instance.SendPacket(new PacketPlayerRotation(player));
                this.lastHeadYaw = player.GetHeadYaw();
                this.lastHeadPitch = player.GetHeadPitch();
            }
        }

        private bool ShouldSendRotation()
        {
            return player.GetHeadYaw() != this.lastHeadYaw || player.GetHeadPitch() != this.lastHeadPitch;
        }

        private void SendPlayerPosition()
        {
            posTicks++;

            if(posTicks % POSITION_SEND_RATE == 0)
            {
                NetClient.instance.SendPacket(new PacketPlayerPosition(player));
                posTicks = 0;
            }
        }
    }
}
