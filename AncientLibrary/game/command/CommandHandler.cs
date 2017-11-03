using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.command
{
    public class CommandHandler
    {
        public static Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public static void Initialize()
        {
            commands.Add("spawn", new CommandSpawnEntity());
            commands.Add("heal", new CommandHealPlayer());
            commands.Add("togglerain", new CommandToggleRain());
        }

        public static ICommand GetCommandFromName(string commandString)
        {
            ICommand command = null;
            commandString = commandString.ToLower();
            commands.TryGetValue(commandString, out command);
            return command;
        }

        public static void HandleText(EntityPlayer sender, string text)
        {
            string[] args = text.Split(' ');
            string commandName = args[0].Remove(0, 1);
            ICommand command = GetCommandFromName(commandName);

            if (command == null)
            {
                Console.WriteLine("Command '"+ commandName + "' not found.");
                return;
            }

            string[] argsWithoutName = new string[args.Length - 1];

            for (int i = 1; i < args.Length; i++)
                argsWithoutName[i - 1] = args[i];

            command.Execute(sender, argsWithoutName);
        }
    }
}
