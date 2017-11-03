using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketDespawnEntity : PacketEntity
    {
        public PacketDespawnEntity()
        { }

        public PacketDespawnEntity(Entity entity) : base(entity)
        { }
    }
}
