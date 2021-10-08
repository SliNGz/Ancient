using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.entity.monster;
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
        private static readonly Dictionary<IDName, Type> entities = new Dictionary<IDName, Type>();

        public static void Initialize()
        {
            InitializeEntity(0, "player", typeof(EntityPlayer));
            InitializeEntity(1, "tortoise", typeof(EntityTortoise));
            InitializeEntity(2, "slime", typeof(EntitySlime));
            InitializeEntity(3, "npc", typeof(EntityNPC));
            InitializeEntity(4, "drop", typeof(EntityDrop));
            InitializeEntity(5, "arrow", typeof(EntityArrow));
            InitializeEntity(6, "staff_projectile", typeof(EntityProjectileStaff));
            InitializeEntity(7, "explosion", typeof(EntityExplosion));
            InitializeEntity(8, "nymu", typeof(EntityNymu));
            InitializeEntity(9, "portal", typeof(EntityPortal));
            InitializeEntity(10, "bee", typeof(EntityBee));
        }

        public static void InitializeEntity(int id, string name, Type type)
        {
            entities.Add(new IDName(id, name), type);
        }

        public static Entity CreateEntityFromTypeID(int typeID)
        {
            Type type = entities.FirstOrDefault(x => x.Key.id == typeID).Value;

            if (type != null)
                return (Entity)Activator.CreateInstance(type, World.world);

            return null;
        }

        public static int GetTypeIDFromEntity(Entity entity)
        {
            try
            {
                return entities.First(x => x.Key.name == entity.GetEntityName()).Key.id;
            }
            catch (InvalidOperationException)
            { }

            return -1;
        }
    }

    public struct IDName
    {
        public int id;
        public string name;

        public IDName(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
