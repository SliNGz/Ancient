using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.entity.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerInputHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerInput playerInput = (PacketPlayerInput)packet;

            EntityPlayerOnline player = (EntityPlayerOnline)netConnection.player;
            player.currentInput = playerInput.GetMovementVector();
            player.SetJumping(playerInput.IsJumping());
            player.SetRunning(playerInput.IsRunning());
        }
    }
}
