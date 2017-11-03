using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    class CommandHealPlayer : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            int heal = int.Parse(args[0]);
            sender.AddHealth(heal);
        }
    }
}
