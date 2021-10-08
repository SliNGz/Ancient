using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancientlib.game.item;
using ancientlib.game.init;

namespace ancientlib.game.utils.chat
{
    public class ChatComponentItem : ChatComponent
    {
        private Item item;
        private float yaw;

        public ChatComponentItem()
        { }

        public ChatComponentItem(Item item)
        {
            this.item = item;
        }

        public override void Update()
        {
            base.Update();
            this.yaw += 1 / 128F;
        }

        public override void Read(BinaryReader reader)
        {
            int id = reader.ReadInt32();
            this.item = Items.GetItemFromID(id);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Items.GetIDFromItem(item));
        }

        public Item GetItem()
        {
            return this.item;
        }

        public float GetYaw()
        {
            return this.yaw;
        }
    }
}
