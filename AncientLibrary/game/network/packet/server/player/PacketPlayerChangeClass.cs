using ancientlib.game.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerChangeClass : Packet
    {
        private int classID;

        public PacketPlayerChangeClass()
        { }

        public PacketPlayerChangeClass(Class _class)
        {
            this.classID = Classes.GetIDFromClass(_class);
        }

        public override void Read(BinaryReader reader)
        {
            this.classID = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(classID);
        }

        public Class GetClass()
        {
            return Classes.GetClassFromID(classID);
        }
    }
}
