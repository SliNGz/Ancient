using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.player;
using ancient.game.entity.player;
using ancient.game.client.world;
using ancientlib.game.entity;

namespace ancient.game.client.network.packet.handler.player
{
    class PlayerMountPetHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerMountPet mountPacket = (PacketPlayerMountPet)packet;

            MountAction mountAction = mountPacket.GetMountAction();

            EntityPlayer player = (EntityPlayer)Ancient.ancient.world.GetEntityFromID(mountPacket.GetPlayerID());

            if (mountAction == MountAction.MOUNT)
                player.Mount((EntityMount)Ancient.ancient.world.GetEntityFromID(mountPacket.GetMountID()));
            else
                player.Dismount();
        }
    }
}
