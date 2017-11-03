using ancient.game.entity;
using ancientlib.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.entity
{
    public class NetEntityManager
    {
        private List<NetEntity> netEntities;
        private Queue<NetEntity> unloadedNetEntities;

        public NetEntityManager()
        {
            this.netEntities = new List<NetEntity>();
            this.unloadedNetEntities = new Queue<NetEntity>();
        }

        public void Update()
        {
            for (int i = 0; i < netEntities.Count; i++)
                netEntities[i].Update();

            while (unloadedNetEntities.Count > 0)
                netEntities.Remove(unloadedNetEntities.Dequeue());
        }

        public void AddNetEntity(Entity entity)
        {
            netEntities.Add(NetEntities.CreateNetEntityFromEntity(entity));
        }

        public void RemoveNetEntity(Entity entity)
        {
            unloadedNetEntities.Enqueue(netEntities.First(x => x.GetEntity() == entity));
        }

        public NetEntity GetNetEntity(Entity entity)
        {
            for (int i = 0; i < netEntities.Count; i++)
            {
                NetEntity netEntity = netEntities[i];

                if (netEntity.GetEntity() == entity)
                    return netEntity;
            }

            return null;
        }
    }
}
