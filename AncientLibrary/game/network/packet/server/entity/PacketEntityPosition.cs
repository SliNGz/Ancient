using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;

namespace ancientlib.game.network.packet.server.entity
{
    public class PacketEntityPosition : PacketEntity
    {
        private Vector3 position;

        public PacketEntityPosition()
        { }

        public PacketEntityPosition(Entity entity) : base(entity)
        {
            this.position = entity.GetPosition();
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.position = reader.ReadVector3();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(position);
        }

        public Vector3 GetPosition()
        {
            return this.position;
        }
    }
}
