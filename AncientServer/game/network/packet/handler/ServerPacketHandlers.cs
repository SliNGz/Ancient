using ancientlib.game.network.packet.client.handshake;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.network.packet.common.status;
using ancientlib.game.network.packet.handler;
using ancientserver.game.network.packet.handler.handshake;
using ancientserver.game.network.packet.handler.player;
using ancientserver.game.network.packet.handler.status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientserver.game.network.packet.handler
{
    public class ServerPacketHandlers : PacketHandlers
    {
        public static void Initialize()
        {
            packetHandlers.Add(typeof(PacketHandshake), new HandshakeHandler());
            packetHandlers.Add(typeof(PacketCharacterCreation), new CharacterCreationHandler());
            packetHandlers.Add(typeof(PacketDisconnect), new DisconnectServerHandler());
            packetHandlers.Add(typeof(PacketPlayerInput), new PlayerInputHandler());
            packetHandlers.Add(typeof(PacketPlayerRotation), new PlayerRotationHandler());
            packetHandlers.Add(typeof(PacketPlayerPosition), new PlayerPositionServerHandler());
            packetHandlers.Add(typeof(PacketPlayerUseItem), new PlayerUseItemHandler());
            packetHandlers.Add(typeof(PacketPlayerChangeSlot), new PlayerChangeSlotHandler());
            packetHandlers.Add(typeof(PacketPlayerRespawn), new PlayerRespawnServerHandler());
            packetHandlers.Add(typeof(PacketPlayerChat), new PlayerChatHandler());
        }
    }
}
