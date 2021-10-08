using ancient.game.entity;
using ancientlib.game.entity;
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

        #region Properties
        public Entity Entity
        {
            get { return entity; }
        }
        #endregion

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
            this.entity = Entities.CreateEntityFromTypeID(typeID);
            this.entity.SetID(id);
            entity.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(typeID);
            entity.Write(writer);
        }
    }
}
