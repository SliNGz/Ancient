using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.world;
using Microsoft.Xna.Framework;
using ancient.game.world;

namespace ancient.game.client.network.packet.handler.world
{
    class CreateWorldHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketCreateWorld worldPacket = (PacketCreateWorld)packet;

            Ancient.ancient.CreateWorld(worldPacket.GetSeed(), true);
            Ancient.ancient.world.SetSpawnPoint(new Vector3(worldPacket.GetXSpawn(), worldPacket.GetYSpawn(), worldPacket.GetZSpawn()));

            //Ancient.ancient.user.SetCharactersArray(worldPacket.GetCharactersArray());
            //Ancient.ancient.guiManager.characterSelection.character = Ancient.ancient.user.GetCharactersArray().GetCharacter(0);
        }
    }
}
