using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    class CommandExp : CommandChangeValue
    {
        public override void AddValue(EntityPlayer sender, int add)
        {
            sender.AddExp(add);
        }

        public override void SetValue(EntityPlayer sender, int value)
        {
            sender.SetExp(value);
        }
    }
}
