using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.world.chunk;
using ancientlib.game.init;
using ancientlib.game.world.chunk;

namespace ancientlib.game.entity.ai
{
    public class EntityAIFlyToBlock : EntityAI
    {
        private EntityFlying entity;
        private Block block;
        private double timeElapsed;
        private int x;
        private int y;
        private int z;

        public EntityAIFlyToBlock(EntityFlying entity, int priority, Block block) : base(priority)
        {
            this.entity = entity;
            this.block = block;
        }

        public override void Execute()
        {
            timeElapsed = entity.GetWorld().rand.Next(10, 14);

            Chunk chunk = entity.GetChunk();
            Chunk[] neighbors = chunk.GetNeighbors();

            Chunk searchChunk = chunk;

            for (int i = -1; i < neighbors.Length; i++)
            {
                if (i >= 0)
                    searchChunk = neighbors[i];

                BlockArray blocks = searchChunk.GetBlockArray();

                for (int j = 0; j < 4096; j++)
                {
                    if (blocks.GetBlock(j) == block)
                    {
                        this.x = searchChunk.GetX() + j / (16 * 16);
                        this.y = searchChunk.GetY() + (j % (16 * 16)) / 16;
                        this.z = searchChunk.GetZ() + j % 16;

                        i = neighbors.Length;
                        break;
                    }
                }
            }

            Console.WriteLine(new Vector3(x, y, z));

            this.entity.GetPathFinder().SetPath(x, y, z);
        }

        public override bool ShouldExecute()
        {
            return entity.GetWorld().rand.Next(4) == 0;
        }

        public override bool ShouldUpdate()
        {
            return timeElapsed > 0;
        }

        public override void Stop()
        {
            entity.GetPathFinder().ClearPath();
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
