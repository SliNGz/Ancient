using ancient.game.client.network.packet.handler.entity;
using ancient.game.client.network.packet.handler.player;
using ancient.game.client.network.packet.handler.status;
using ancient.game.client.network.packet.handler.world;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.network.packet.common.status;
using ancientlib.game.network.packet.handler;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.network.packet.server.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.network.packet.handler
{
    public class ClientPacketHandlers : PacketHandlers
    {
        public static void Initialize()
        {
            packetHandlers.Add(typeof(PacketDisconnect), new DisconnectClientHandler());
            packetHandlers.Add(typeof(PacketCreateWorld), new CreateWorldHandler());
            packetHandlers.Add(typeof(PacketCharacterCreationStatus), new CharacterCreationStatusHandler());
            packetHandlers.Add(typeof(PacketSpawnEntity), new EntitySpawnHandler());
            packetHandlers.Add(typeof(PacketDespawnEntity), new EntityDespawnHandler());
            packetHandlers.Add(typeof(PacketEntityPosition), new EntityPositionHandler());
            packetHandlers.Add(typeof(PacketEntityRotation), new EntityRotationHandler());
            packetHandlers.Add(typeof(PacketPlayerPosition), new PlayerPositionClientHandler());
            packetHandlers.Add(typeof(PacketDestroyBlock), new BlockDestructionHandler());
            packetHandlers.Add(typeof(PacketPlaceBlock), new BlockPlacementHandler());
            packetHandlers.Add(typeof(PacketPlayerItemAction), new PlayerItemActionHandler());
            packetHandlers.Add(typeof(PacketEntityHealth), new EntityHealthHandler());
            packetHandlers.Add(typeof(PacketPlayerTamePet), new PlayerTamePetHandler());
            packetHandlers.Add(typeof(PacketPlayerMountPet), new PlayerMountPetHandler());
            packetHandlers.Add(typeof(PacketPlayerRespawn), new PlayerRespawnClientHandler());
            packetHandlers.Add(typeof(PacketDamageEntity), new EntityDamagedHandler());
            packetHandlers.Add(typeof(PacketToggleRain), new ToggleRainHandler());
        }
    }
}
