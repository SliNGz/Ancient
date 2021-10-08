using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.handshake;
using ancientlib.game.constants;
using ancientlib.game.utils;
using ancientlib.game.network.packet.server.world;
using ancient.game.entity.player;
using ancientlib.game.world;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.player;
using ancient.game.entity;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.network.packet.common.status;
using ancientlib.AncientService;
using ancientlib.game.user;
using ancientserver.game.utils;

namespace ancientserver.game.network.packet.handler.handshake
{
    class HandshakeHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketHandshake handshake = (PacketHandshake)packet;

            ancientlib.game.user.User user = netConnection.GetUser();

            user.SetUsername(handshake.GetUsername());
            user.SetPassword(handshake.GetPassword());

            if (AncientServer.ancientServer.service.IsUserValid(user.GetUsername(), user.GetPassword()))
            {
                if (handshake.GetVersion() == GameConstants.GAME_VERSION)
                    AcceptClient(netConnection);
                else
                {
                    string message = "";

                    if (handshake.GetVersion() < GameConstants.GAME_VERSION)
                    {
                        message = "Client outdated.";
                        ConsoleExtensions.WriteLine(ConsoleColor.Yellow, "An outdated client failed to connect: " + netConnection);
                    }
                    else
                    {
                        message = "Server outdated.";
                        ConsoleExtensions.WriteLine(ConsoleColor.Yellow, "Server is outdated! A client has failed to connect: " + netConnection);
                    }

                    netConnection.SendPacket(new PacketDisconnect(message));
                }
            }
            else
            {
                ConsoleExtensions.WriteLine(ConsoleColor.Yellow, "A client with wrong username or password tried to connect: " + netConnection);
                netConnection.SendPacket(new PacketDisconnect("Username or password are wrong."));
            }
        }

        private void AcceptClient(NetConnection netConnection)
        {
            AncientServiceServerUtils.CreateUserDirectory(netConnection.GetUser().GetUsername());

            WorldServer world = AncientServer.ancientServer.world;
            EntityPlayerOnline player = new EntityPlayerOnline(world, netConnection);

            NetServer.instance.AddNetConnection(netConnection);
            netConnection.StartReadingPackets();

           // if (AncientServiceServerUtils.UserHasCharacters(netConnection.GetUser().GetUsername()))
                netConnection.GetUser().GetCharactersArray().SetCharacters(AncientServiceServerUtils.GetUserCharacters(netConnection.GetUser().GetUsername()));

            SendCreateWorld(netConnection, world);
            RequestCharacterSelection(netConnection);
        }

        private void SendCreateWorld(NetConnection netConnection, WorldServer world)
        {
            netConnection.SendPacket(new PacketCreateWorld(world, netConnection));

            for (int i = 0; i < world.entityList.Count; i++)
                netConnection.SendPacket(new PacketSpawnEntity(world.entityList[i]));
        }

        private void RequestCharacterSelection(NetConnection netConnection)
        {
            netConnection.stage = ConnectionStage.CHARACTER_SELECTION;
            netConnection.SendPacket(new PacketCharacterStatus(CharacterStatus.REQUEST_CHARACTER_SELECTION));
        }
    }
}
