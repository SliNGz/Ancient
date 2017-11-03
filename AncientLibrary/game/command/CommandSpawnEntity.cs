using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.entity;
using ancient.game.entity;

namespace ancientlib.game.command
{
    class CommandSpawnEntity : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Wrong usage of command");
                return;
            }

            int id = int.Parse(args[0]);

            Entity entity = Entities.CreateEntityFromTypeID(id, sender.GetWorld());

            if (entity == null)
                Console.WriteLine("No entity with id: " + id);

            entity.SetPosition(sender.GetPosition());
            sender.GetWorld().SpawnEntity(entity);
        }
    }
}
