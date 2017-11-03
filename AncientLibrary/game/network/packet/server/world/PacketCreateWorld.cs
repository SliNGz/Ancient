using ancient.game.entity.player;
using ancient.game.utils;
using ancient.game.world;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.world
{
    public class PacketCreateWorld : Packet
    {
        private int seed;
        private int xSpawn;
        private int ySpawn;
        private int zSpawn;
        private int playerID;

        public PacketCreateWorld()
        { }

        public PacketCreateWorld(World world, EntityPlayer player)
        {
            this.seed = world.GetSeed();
            this.xSpawn = (int)world.GetSpawnPoint().X;
            this.ySpawn = (int)world.GetSpawnPoint().Y;
            this.zSpawn = (int)world.GetSpawnPoint().Z;
            this.playerID = player.GetID();
        }

        public override void Read(BinaryReader reader)
        {
            this.seed = reader.ReadInt32();
            reader.ReadPositionInt(out xSpawn, out ySpawn, out zSpawn);
            this.playerID = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(seed);
            writer.Write(xSpawn, ySpawn, zSpawn);
            writer.Write(playerID);
        }

        public int GetSeed()
        {
            return this.seed;
        }

        public int GetXSpawn()
        {
            return this.xSpawn;
        }

        public int GetYSpawn()
        {
            return this.ySpawn;
        }

        public int GetZSpawn()
        {
            return this.zSpawn;
        }

        public int GetPlayerID()
        {
            return this.playerID;
        }
    }
}
