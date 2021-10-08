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
using ancientlib.game.user;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.utils;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerSelectCharacterHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketSelectCharacter selectCharacterPacket = (PacketSelectCharacter)packet;

            CharactersArray characters = netConnection.GetUser().GetCharactersArray();
            EntityPlayer selectedCharacter = characters.GetCharacter(selectCharacterPacket.GetCharacterIndex());

            netConnection.player.CopyFrom(selectedCharacter);

            netConnection.player.GetWorld().SpawnEntity(netConnection.player);
            netConnection.SendPacket(new PacketCharacterStatus(CharacterStatus.SELECT_CHARACTER));
            netConnection.stage = ConnectionStage.INGAME;

            ConsoleExtensions.WriteLine(ConsoleColor.Yellow, netConnection.player.GetName() + " has connected: " + netConnection);
        }
    }
}
