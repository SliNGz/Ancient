using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.entity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.entity
{
    public class EntityList : List<Entity>
    {
        private World world;
        private Queue<Entity> unloadEntities;

        public EntityList(World world)
        {
            this.world = world;
            this.unloadEntities = new Queue<Entity>();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Count; i++)
            {
                Entity entity = this[i];

                entity.Update(gameTime);

                if (entity.ShouldDespawn())
                {
                    if (entity is EntityPlayer)
                    {
                        ((EntityPlayer)entity).SetHealth(0);
                        entity.SetVelocity(Vector3.Zero);
                        continue;
                    }

                    unloadEntities.Enqueue(entity);
                }
            }

            while (unloadEntities.Count > 0)
            {
                Entity entity = unloadEntities.Dequeue();

                world.OnDespawnEntity(entity);
                Remove(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            unloadEntities.Enqueue(entity);
        }
    }
}
