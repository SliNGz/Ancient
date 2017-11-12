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
            int id = 0;
            int.TryParse(args[0], out id);

            Entity entity = Entities.CreateEntityFromTypeID(id, sender.GetWorld());

            if (entity == null)
            {
                Console.WriteLine("No entity with id: " + id);
                return;
            }

            entity.SetPosition(sender.GetPosition());
            sender.GetWorld().SpawnEntity(entity);
        }

        public int GetMinArgs()
        {
            return 1;
        }
    }
}
