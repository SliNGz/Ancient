using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    class CommandSpeed : CommandChangeValueFloat
    {
        public override void AddValue(EntityPlayer sender, float add)
        {
            sender.AddRunningSpeed(add);
        }

        public override void SetValue(EntityPlayer sender, float value)
        {
            sender.SetRunningSpeed(value);
        }
    }
}
