using ancient.game.entity;
using ancient.game.entity.player;
using ancientlib.game.entity.npc;
using ancientlib.game.entity.passive;
using ancientlib.game.entity.projectile;
using ancientlib.game.entity.world;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity
{
    public class Entities
    {
        private static readonly Dictionary<int, Type> entities = new Dictionary<int, Type>();

        public static void Initialize()
        {
            entities.Add(0, typeof(EntityPlayer));

            entities.Add(1, typeof(EntityTortoise));
            entities.Add(2, typeof(EntitySlime));
            entities.Add(3, typeof(EntityNPC));

            entities.Add(4, typeof(EntityDrop));
            entities.Add(5, typeof(EntityArrow));
            entities.Add(6, typeof(EntityProjectileStaff));
        }

        public static Entity CreateEntityFromTypeID(int typeID, params object[] paramArray)
        {
            Type type = null;

            if (entities.TryGetValue(typeID, out type))
                return (Entity)Activator.CreateInstance(type, paramArray);

            return null;
        }

        public static int GetTypeIDFromEntity(Entity entity)
        {
            return entities.FirstOrDefault(x => x.Value == entity.GetType()).Key;
        }

        public static Type GetTypeFromTypeID(int typeID)
        {
            Type type = null;
            entities.TryGetValue(typeID, out type);

            return type;
        }
    }
}
