using ancient.game.entity.player;
using ancientlib.game.utils.chat;
using Microsoft.Xna.Framework;
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
            commands.Add("health", new CommandHealth());
            commands.Add("togglerain", new CommandToggleRain());
            commands.Add("exp", new CommandExp());
            commands.Add("additem", new CommandAddItem());
            commands.Add("speed", new CommandSpeed());
            commands.Add("str", new CommandStr());
            commands.Add("wsd", new CommandWsd());
            commands.Add("dex", new CommandDex());
            commands.Add("luk", new CommandLuk());
            commands.Add("class", new CommandClass());
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
                sender.GetWorld().AddChatComponent(new ChatComponentText("Command '" + commandName + "' not found.", Color.Red));
                return;
            }

            string[] commandArgs = new string[args.Length - 1];

            for (int i = 1; i < args.Length; i++)
                commandArgs[i - 1] = args[i];

            if (command.GetMinArgs() <= commandArgs.Length)
                command.Execute(sender, commandArgs);
            else
                Console.WriteLine("Too few arguments");
        }
    }
}
