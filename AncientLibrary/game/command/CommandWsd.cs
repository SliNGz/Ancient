using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    class CommandWsd : CommandChangeValue
    {
        public override void AddValue(EntityPlayer sender, int add)
        {
            sender.AddWsd(add);
        }

        public override void SetValue(EntityPlayer sender, int value)
        {
            sender.SetWsd(value);
        }
    }
}
