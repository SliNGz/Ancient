using ancientlib.game.network.packet.client.handshake;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.network.packet.common.chat;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.network.packet.common.status;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.network.packet.server.world;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet
{
    class Packets
    {
        private static Dictionary<byte, Type> clientPackets = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> serverPackets = new Dictionary<byte, Type>();

        public static void Initialize()
        {
            // Client
            clientPackets.Add(0x00, typeof(PacketHandshake));
            clientPackets.Add(0x01, typeof(PacketCreateCharacter));
            clientPackets.Add(0x02, typeof(PacketSelectCharacter));
            clientPackets.Add(0x03, typeof(PacketDisconnect));
            clientPackets.Add(0x04, typeof(PacketPlayerInput));
            clientPackets.Add(0x05, typeof(PacketPlayerRotation));
            clientPackets.Add(0x06, typeof(PacketPlayerPosition));
            clientPackets.Add(0x07, typeof(PacketPlayerUseItem));
            clientPackets.Add(0x08, typeof(PacketPlayerChangeSlot));
            clientPackets.Add(0x09, typeof(PacketPlayerRespawn));
            clientPackets.Add(0x10, typeof(PacketChatComponent));
            clientPackets.Add(0x11, typeof(PacketPlayerUseSpecial));
            clientPackets.Add(0x12, typeof(PacketPlayerDropItem));

            //Server
            serverPackets.Add(0x00, typeof(PacketDisconnect));
            serverPackets.Add(0x01, typeof(PacketCreateWorld));
            serverPackets.Add(0x02, typeof(PacketCharacterStatus));
            serverPackets.Add(0x03, typeof(PacketSpawnEntity));
            serverPackets.Add(0x04, typeof(PacketDespawnEntity));
            serverPackets.Add(0x05, typeof(PacketEntityPosition));
            serverPackets.Add(0x06, typeof(PacketEntityRotation));
            serverPackets.Add(0x07, typeof(PacketPlayerPosition));
            serverPackets.Add(0x08, typeof(PacketDestroyBlock));
            serverPackets.Add(0x09, typeof(PacketPlaceBlock));
            serverPackets.Add(0x10, typeof(PacketPlayerItemAction));
            serverPackets.Add(0x11, typeof(PacketEntityHealth));
            serverPackets.Add(0x12, typeof(PacketPlayerTamePet));
            serverPackets.Add(0x13, typeof(PacketPlayerMountPet));
            serverPackets.Add(0x14, typeof(PacketPlayerRespawn));
            serverPackets.Add(0x15, typeof(PacketDamageEntity));
            serverPackets.Add(0x16, typeof(PacketPlayerChangeClass));
            serverPackets.Add(0x17, typeof(PacketToggleRain));
            serverPackets.Add(0x18, typeof(PacketExplosion));
            serverPackets.Add(0x19, typeof(PacketChatComponent));
            serverPackets.Add(0x20, typeof(PacketPlayerUseSpecial));
            serverPackets.Add(0x21, typeof(PacketEntityHeadRotation));
            serverPackets.Add(0x22, typeof(PacketPlayerChangeExp));
            serverPackets.Add(0x23, typeof(PacketPlayerChangeLevel));
            serverPackets.Add(0x24, typeof(PacketPlayerMaxHealth));
        }

        public static Packet GetSendPacketFromID(byte id)
        {
            Type type = null;

            if (NetConnection.IsServer())
                serverPackets.TryGetValue(id, out type);
            else
                clientPackets.TryGetValue(id, out type);

            return type == null ? null : (Packet)Utils.CreateObjectFromType(type).Invoke();
        }

        public static Packet GetRecievePacketFromID(byte id)
        {
            Type type = null;

            if (NetConnection.IsServer())
                clientPackets.TryGetValue(id, out type);
            else
                serverPackets.TryGetValue(id, out type);

            return type == null ? null : (Packet)Utils.CreateObjectFromType(type).Invoke();
        }

        public static byte GetIDFromSendPacket(Packet packet)
        {
            if (NetConnection.IsServer())
                return serverPackets.FirstOrDefault(x => x.Value == packet.GetType()).Key;

            return clientPackets.FirstOrDefault(x => x.Value == packet.GetType()).Key;
        }
    }
}
