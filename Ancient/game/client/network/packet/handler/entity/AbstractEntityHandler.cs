using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.entity;
using ancient.game.entity;

namespace ancient.game.client.network.packet.handler.entity
{
    abstract class AbstractEntityHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketEntity entityPacket = (PacketEntity)packet;

            Entity entity = Ancient.ancient.world.GetEntityFromID(entityPacket.GetID());

            if (entity == null)
            {
                Console.WriteLine("Entity null: " + packet);
                return;
            }

            HandlePacket(entity, entityPacket, netConnection);
        }

        public abstract void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection);
    }
}
