using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework;

namespace ancientlib.game.command
{
    class CommandClass : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            string className = args[0];

            Class _class = Classes.GetClassFromName(className);

            if (_class == null)
            {
                sender.GetWorld().AddChatComponent(new ChatComponentText(className + " doesn't exist.", Color.Red));
                return;
            }

            sender.SetClass(_class);
        }

        public int GetMinArgs()
        {
            return 1;
        }
    }
}
