using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.handler
{
    public class PacketHandlers
    {
        protected static Dictionary<Type, IPacketHandler> packetHandlers = new Dictionary<Type, IPacketHandler>();

        public static IPacketHandler GetPacketHandlerFromPacket(Packet packet)
        {
            IPacketHandler ph = null;
            packetHandlers.TryGetValue(packet.GetType(), out ph);

            return ph;
        }
    }
}
