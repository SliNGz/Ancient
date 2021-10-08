using ancientlib.game.constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.handshake
{
    public class PacketHandshake : Packet
    {
        private double version;
        private string username;
        private string password;

        public PacketHandshake()
        { }

        public PacketHandshake(NetConnection netConnection)
        {
            this.version = GameConstants.GAME_VERSION;
            this.username = netConnection.GetUser().GetUsername();
            this.password = netConnection.GetUser().GetPassword();
        }

        public override void Read(BinaryReader reader)
        {
            this.version = reader.ReadDouble();
            this.username = reader.ReadString();
            this.password = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(username);
            writer.Write(password);
        }

        public double GetVersion()
        {
            return this.version;
        }

        public string GetUsername()
        {
            return this.username;
        }

        public string GetPassword()
        {
            return this.password;
        }
    }
}
