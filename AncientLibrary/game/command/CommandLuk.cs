using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    class CommandLuk : CommandChangeValue
    {
        public override void AddValue(EntityPlayer sender, int add)
        {
            sender.AddLuk(add);
        }

        public override void SetValue(EntityPlayer sender, int value)
        {
            sender.SetLuk(value);
        }
    }
}
