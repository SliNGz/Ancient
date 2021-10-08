using ancientlib.game.network.packet.client.handshake;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.network.packet.common.chat;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.network.packet.common.status;
using ancientlib.game.network.packet.handler;
using ancientserver.game.network.packet.handler.chat;
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
            packetHandlers.Add(typeof(PacketCreateCharacter), new PlayerCreateCharacterHandler());
            packetHandlers.Add(typeof(PacketSelectCharacter), new PlayerSelectCharacterHandler());
            packetHandlers.Add(typeof(PacketDisconnect), new DisconnectServerHandler());
            packetHandlers.Add(typeof(PacketPlayerInput), new PlayerInputHandler());
            packetHandlers.Add(typeof(PacketPlayerRotation), new PlayerRotationHandler());
            packetHandlers.Add(typeof(PacketPlayerPosition), new PlayerPositionServerHandler());
            packetHandlers.Add(typeof(PacketPlayerUseItem), new PlayerUseItemHandler());
            packetHandlers.Add(typeof(PacketPlayerChangeSlot), new PlayerChangeSlotHandler());
            packetHandlers.Add(typeof(PacketPlayerRespawn), new PlayerRespawnServerHandler());
            packetHandlers.Add(typeof(PacketChatComponent), new ChatComponentServerHandler());
            packetHandlers.Add(typeof(PacketPlayerUseSpecial), new PlayerUseSpecialServerHandler());
            packetHandlers.Add(typeof(PacketPlayerDropItem), new PlayerDropItemHandler());
        }
    }
}
