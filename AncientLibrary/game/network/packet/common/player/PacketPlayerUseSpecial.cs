using ancient.game.entity.player;
using ancientlib.game.network.packet.server.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.common.player
{
    public class PacketPlayerUseSpecial : PacketEntity
    {
        public PacketPlayerUseSpecial()
        { }

        public PacketPlayerUseSpecial(EntityPlayer player) : base(player)
        { }
    }
}
