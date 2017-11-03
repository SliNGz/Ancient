using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet
{
    public abstract class Packet
    {
        public abstract void Read(BinaryReader reader);

        public abstract void Write(BinaryWriter writer);
    }
}
