using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.player;

namespace ancient.game.client.network.packet.handler.player
{
    class CharacterCreationStatusHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketCharacterCreationStatus characterStatus = (PacketCharacterCreationStatus)packet;

            switch (characterStatus.GetStatus())
            {
                case CharacterStatus.REQUEST_CHARACTER_CREATION:
                    HandleCharacterCreationRequest();
                    break;

                case CharacterStatus.SPAWN_CHARACTER:
                    HandleSpawnCharacter();
                    break;
            }
        }

        private void HandleCharacterCreationRequest()
        {
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.characterCreation);
        }

        private void HandleSpawnCharacter()
        {
            Ancient.ancient.SpawnPlayer();
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.ingame);
            Ancient.ancient.inputManager.onlineInput.canSendInput = true;

            Ancient.ancient.player.SetHandSlot(0);
        }
    }
}
