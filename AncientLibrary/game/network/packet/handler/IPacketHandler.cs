using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.handler
{
    public interface IPacketHandler
    {
        void HandlePacket(Packet packet, NetConnection netConnection);
    }
}
