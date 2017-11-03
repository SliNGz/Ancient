using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;
using ancient.game.entity.player;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.utils;

namespace ancientserver.game.network.packet.handler.player
{
    class CharacterCreationHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketCharacterCreation characterCreation = (PacketCharacterCreation)packet;

            EntityPlayer player = netConnection.player;

            player.SetName(characterCreation.GetName());
            player.SetSkinColor(characterCreation.GetSkinColor());
            player.SetHairID(characterCreation.GetHairID());
            player.SetHairColor(characterCreation.GetHairColor());
            player.SetEyesID(characterCreation.GetEyesID());
            player.SetEyesColor(characterCreation.GetEyesColor());

            SpawnPlayer(netConnection);
        }

        private void SpawnPlayer(NetConnection netConnection)
        {
            netConnection.player.GetWorld().SpawnEntity(netConnection.player);
            netConnection.SendPacket(new PacketCharacterCreationStatus(CharacterStatus.SPAWN_CHARACTER));
            netConnection.stage = ConnectionStage.INGAME;

            ConsoleExtensions.WriteLine(ConsoleColor.Yellow, netConnection.player.GetName() + " has connected: " + netConnection);
        }
    }
}
