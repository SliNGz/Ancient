using ancientlib.game.item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerItemAction : Packet
    {
        private ItemStack itemStack;
        private ItemAction itemAction;

        public PacketPlayerItemAction()
        { }

        public PacketPlayerItemAction(ItemStack itemStack, ItemAction itemAction)
        {
            this.itemStack = itemStack;
            this.itemAction = itemAction;
        }

        public override void Read(BinaryReader reader)
        {
            this.itemAction = (ItemAction)reader.ReadByte();
            this.itemStack = new ItemStack(null);
            itemStack.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)itemAction);
            itemStack.Write(writer);
        }

        public ItemStack GetItemStack()
        {
            return this.itemStack;
        }

        public ItemAction GetItemAction()
        {
            return this.itemAction;
        }
    }

    public enum ItemAction
    {
        ADD_ITEM = 0x00,
        REMOVE_ITEM = 0x01,
    }
}
