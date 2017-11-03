using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    class CommandToggleRain : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            sender.GetWorld().SetRaining(!sender.GetWorld().IsRaining());
        }
    }
}
