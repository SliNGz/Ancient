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

namespace ancientserver.game.network.packet.handler.handshake
{
    class HandshakeHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketHandshake handshake = (PacketHandshake)packet;

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

        private void AcceptClient(NetConnection netConnection)
        {
            WorldServer world = AncientServer.ancientServer.world;
            EntityPlayerOnline player = new EntityPlayerOnline(world, netConnection);

            NetServer.instance.AddNetConnection(netConnection);
            netConnection.StartReadingPackets();
            SendCreateWorld(netConnection, world, player);
            RequestCharacterCreation(netConnection);
        }

        private void SendCreateWorld(NetConnection netConnection, WorldServer world, EntityPlayer player)
        {
            netConnection.SendPacket(new PacketCreateWorld(world, player));

            for (int i = 0; i < world.entityList.Count; i++)
                netConnection.SendPacket(new PacketSpawnEntity(world.entityList[i]));
        }

        private void RequestCharacterCreation(NetConnection netConnection)
        {
            netConnection.stage = ConnectionStage.CHARACTER_CREATION;
            netConnection.SendPacket(new PacketCharacterCreationStatus(CharacterStatus.REQUEST_CHARACTER_CREATION));
        }
    }
}
