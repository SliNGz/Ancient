using ancient.game.world.chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.chunk
{
    public class LightMap
    {
        private Chunk chunk;

        private byte[] lightMap;

        public LightMap(Chunk chunk)
        {
            this.chunk = chunk;
            this.lightMap = new byte[4096];
        }

        public int GetSunlight(int x, int y, int z)
        {
            return lightMap[x * 16 * 16 + y * 16 + z] >> 4;
        }

        public void SetSunlight(int x, int y, int z, int sunlight)
        {
            this.lightMap[x * 16 * 16 + y * 16 + z] = (byte)(GetBlocklight(x, y, z) | (sunlight << 4));
        }

        public int GetBlocklight(int x, int y, int z)
        {
            return lightMap[x * 16 * 16 + y * 16 + z] & 0xF;
        }

        public void SetBlocklight(int x, int y, int z, int blocklight)
        {
            this.lightMap[x * 16 * 16 + y * 16 + z] = (byte)(blocklight | (GetSunlight(x, y, z) << 4));
        }

        public int GetSunlight(int index)
        {
            return lightMap[index] >> 4;
        }

        public void SetSunlight(int index, int sunlight)
        {
            this.lightMap[index] = (byte)(GetBlocklight(index) | (sunlight << 4));
        }

        public int GetBlocklight(int index)
        {
            return lightMap[index] & 0xF;
        }

        public void SetBlocklight(int index, int blocklight)
        {
            this.lightMap[index] = (byte)(blocklight | (GetSunlight(index) << 4));
        }
    }
}
