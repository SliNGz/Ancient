using ancient.game.entity;
using ancient.game.world;
using ancientlib.game.entity;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketSpawnEntity : PacketEntity
    {
        private Entity entity;
        private int typeID;

        public PacketSpawnEntity()
        { }

        public PacketSpawnEntity(Entity entity) : base(entity)
        {
            this.entity = entity;
            this.typeID = Entities.GetTypeIDFromEntity(entity);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.typeID = reader.ReadInt32();
            this.entity = Entities.CreateEntityFromTypeID(typeID, World.world);
            this.entity.SetID(id);
            entity.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(typeID);
            entity.Write(writer);
        }

        public Entity GetEntity()
        {
            return this.entity;
        }
    }
}
