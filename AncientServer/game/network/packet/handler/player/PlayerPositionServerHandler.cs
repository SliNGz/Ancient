using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;
using Microsoft.Xna.Framework;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.constants;
using ancientlib.game.entity.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerPositionServerHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerPosition playerPos = (PacketPlayerPosition)packet;

            Vector3 recievedPos = new Vector3(playerPos.GetX(), playerPos.GetY(), playerPos.GetZ());

            EntityPlayerOnline player = (EntityPlayerOnline)netConnection.player;
            Vector3 velocity = (recievedPos - player.lastPos);
            float horizontalSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Z * velocity.Z) / 2F;
            float verticalSpeed = velocity.Y / 2F;
            player.lastPos = player.GetPosition();

            double speed = player.GetSpeed();
            double jumpSpeed = player.GetBaseJumpSpeed();

            if (player.IsRiding())
            {
                speed = player.GetMount().GetSpeed();
                jumpSpeed = player.GetMount().GetBaseJumpSpeed();
            }

            if (horizontalSpeed > speed + NetConstants.MAX_SPEED_DIFF || verticalSpeed >= jumpSpeed + NetConstants.MAX_SPEED_DIFF || Math.Abs(recievedPos.Y - player.GetY()) > NetConstants.MAX_Y_DIFF)
            {
                Console.WriteLine("DAFUQ");
                netConnection.SendPacket(new PacketPlayerPosition(netConnection.player));
            }
            else
            {
                player.SetPosition(playerPos.GetX(), playerPos.GetY(), playerPos.GetZ());

                if (player.HasMount())
                    player.GetMount().SetPosition(new Vector3(playerPos.GetX(), playerPos.GetY(), playerPos.GetZ()) - player.GetMount().GetMountOffset());
            }
        }
    }
}
