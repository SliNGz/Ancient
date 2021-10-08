using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.init;
using ancientlib.game.item;
using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework;

namespace ancientlib.game.command
{
    class CommandAddItem : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            int id = 0;

            if (int.TryParse(args[0], out id))
            {
                Item item = Items.GetItemFromID(id);

                if (item == null)
                {
                    sender.GetWorld().AddChatComponent(new ChatComponentText("No item with id: " + id, Color.Red));
                    return;
                }

                int amount = 1;

                if (args.Length > 1)
                {
                    if (int.TryParse(args[1], out amount))
                    {
                        if (amount == 0)
                            amount = 1;
                    }
                }

                sender.AddItem(item, amount);
            }
        }

        public int GetMinArgs()
        {
            return 1;
        }
    }
}
