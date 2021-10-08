using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.user;
using ancient.game.utils;
using ancient.game.entity.player;
using System.IO;
using ancient.game.client.gui;
using ancient.game.renderers.world;

namespace ancient.game.client.network.packet.handler.player
{
    class CharacterCreateStatusHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketCharacterStatus characterStatus = (PacketCharacterStatus)packet;

            switch (characterStatus.GetStatus())
            {
                case CharacterStatus.REQUEST_CHARACTER_SELECTION:
                    HandleRequestCharacterSelection();
                    break;

                case CharacterStatus.CREATE_CHARACTER:
                    HandleCreateCharacter();
                    break;

                case CharacterStatus.SELECT_CHARACTER:
                    HandleSelectCharacter();
                    break;

                case CharacterStatus.ERROR_NAME_TAKEN:
                    HandleNameTaken();
                    break;

                case CharacterStatus.ERROR_REACHED_MAX_CHARACTERS:
                    HandleMaxCharactersReached();
                    break;
            }
        }

        private void HandleRequestCharacterSelection()
        {
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.characterSelection);
        }

        private void HandleCreateCharacter()
        {
            NetConnection netConnection = NetClient.instance.GetNetConnection();
            CharactersArray characters = netConnection.GetUser().GetCharactersArray();

            EntityPlayer character = Ancient.ancient.guiManager.characterCreation.GetCharacter();

            characters.AddCharacter(character);
            Ancient.ancient.guiManager.characterSelection.characterIndex = characters.Count - 1;
            Ancient.ancient.guiManager.characterSelection.character = Ancient.ancient.guiManager.characterSelection.charactersArray.GetCharacter(characters.Count - 1);
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.characterSelection);

            Ancient.ancient.world.GetRenderer().characterPNG = character;
        }

        private void HandleSelectCharacter()
        {
            EntityPlayer character = Ancient.ancient.guiManager.characterSelection.character;
            Ancient.ancient.player.CopyFrom(character);

            Ancient.ancient.SpawnPlayer();
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.ingame);
            Ancient.ancient.inputManager.onlineInput.canSendInput = true;

            Ancient.ancient.player.SetHandSlot(0);

            Ancient.ancient.world.GetRenderer().characterPNG = character;
        }

        private void HandleNameTaken()
        {
            Ancient.ancient.guiManager.characterCreation.errorText.SetText("Name taken.");
        }

        private void HandleMaxCharactersReached()
        {
            Ancient.ancient.guiManager.characterCreation.errorText.SetText("Reached max characters.");
        }
    }
}
