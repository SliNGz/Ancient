using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancientlib.game.command
{
    abstract class CommandChangeValueFloat : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            float value = 0;

            float.TryParse(args[0], out value);

            if (args[0].StartsWith("+") || args[0].StartsWith("-"))
                AddValue(sender, value);
            else
                SetValue(sender, value);
        }

        public int GetMinArgs()
        {
            return 1;
        }

        public abstract void AddValue(EntityPlayer sender, float add);

        public abstract void SetValue(EntityPlayer sender, float value);
    }
}
