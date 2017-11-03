using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerRotationHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerRotation rotPacket = (PacketPlayerRotation)packet;

            netConnection.player.SetHeadYaw(rotPacket.GetHeadYaw());
            netConnection.player.SetHeadPitch(rotPacket.GetHeadPitch());

            netConnection.player.SetYaw(rotPacket.GetHeadYaw());
        }
    }
}
