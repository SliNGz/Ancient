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
using ancientlib.game.entity.player;
using ancientlib.game.user;
using ancientserver.game.utils;
using ancientlib.AncientService;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerCreateCharacterHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketCreateCharacter characterCreation = (PacketCreateCharacter)packet;

            if (!AncientServiceServerUtils.CharacterExists(characterCreation.GetName()))
            {
                ancientlib.game.user.User user = netConnection.GetUser();

                if (user.GetCharactersArray().Count >= user.GetCharactersArray().MaxCharacters)
                    SendErrorCharacterStatus(netConnection, CharacterStatus.ERROR_REACHED_MAX_CHARACTERS);
                else
                    CreateCharacter(netConnection, characterCreation);
            }
            else
                SendErrorCharacterStatus(netConnection, CharacterStatus.ERROR_NAME_TAKEN);
        }

        private void CreateCharacter(NetConnection netConnection, PacketCreateCharacter characterCreation)
        {
            EntityPlayer character = new EntityPlayer(AncientServer.ancientServer.world);

            character.SetName(characterCreation.GetName());
            character.SetSkinColor(characterCreation.GetSkinColor());
            character.SetHairID(characterCreation.GetHairID());
            character.SetHairColor(characterCreation.GetHairColor());
            character.SetEyesID(characterCreation.GetEyesID());
            character.SetEyesColor(characterCreation.GetEyesColor());

            netConnection.GetUser().GetCharactersArray().AddCharacter(character);

            AncientServer.ancientServer.service.AddCharacterOfUser(netConnection.GetUser().GetUsername(), AncientServiceServerUtils.GetServicePlayerFromEntityPlayer(character));
          //  AncientServiceServerUtils.SaveCharacter(netConnection.GetUser().GetUsername(), character);

            netConnection.SendPacket(new PacketCharacterStatus(CharacterStatus.CREATE_CHARACTER));
            ConsoleExtensions.WriteLine(ConsoleColor.Green, netConnection.GetUser() + " created a new character: " + character.GetName());
        }

        private void SpawnPlayer(NetConnection netConnection)
        {
     /*       netConnection.player.GetWorld().SpawnEntity(netConnection.player);
            netConnection.SendPacket(new PacketCreateCharacterStatus(CharacterStatus.SPAWN_CHARACTER));
            netConnection.stage = ConnectionStage.INGAME;

            ConsoleExtensions.WriteLine(ConsoleColor.Yellow, netConnection.player.GetName() + " has connected: " + netConnection);*/
        }

        private void SendErrorCharacterStatus(NetConnection netConnection, CharacterStatus status)
        {
            netConnection.SendPacket(new PacketCharacterStatus(status));

            ConsoleExtensions.WriteLine(ConsoleColor.Red, "User: " + netConnection.GetUser() + " failed to create character: " + status);
        }
    }
}
