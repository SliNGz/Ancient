using ancient.game.entity;
using ancient.game.entity.player;
using ancientlib.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.entity
{
    public class NetEntities
    {
        public static Dictionary<Type, Type> netEntities = new Dictionary<Type, Type>();

        public static void Initialize()
        {
            netEntities.Add(typeof(Entity), typeof(NetEntity));
            netEntities.Add(typeof(EntityLiving), typeof(NetEntityLiving));
            netEntities.Add(typeof(EntityPlayer), typeof(NetEntityPlayer));
        }

        public static NetEntity CreateNetEntityFromEntity(Entity entity)
        {
            Type type = null;
            Type entityType = entity.GetType();

            while (entityType.BaseType != null)
            {
                if (netEntities.TryGetValue(entityType, out type))
                    return (NetEntity)Activator.CreateInstance(type, entity);

                entityType = entityType.BaseType;
            }

            return null;
        }
    }
}
