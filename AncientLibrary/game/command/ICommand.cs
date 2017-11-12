using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.command
{
    public interface ICommand
    {
        void Execute(EntityPlayer sender, string[] args);

        int GetMinArgs();
    }
}
