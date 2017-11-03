using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.player;
using ancient.game.entity;
using ancientlib.game.entity;

namespace ancient.game.client.network.packet.handler.player
{
    public class PlayerTamePetHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerTamePet tamePacket = (PacketPlayerTamePet)packet;

            EntityPet pet = (EntityPet)Ancient.ancient.world.GetEntityFromID(tamePacket.GetPetID());
            Ancient.ancient.player.AddPet(pet);
        }
    }
}
