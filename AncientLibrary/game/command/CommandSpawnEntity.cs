using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.entity;
using ancient.game.entity;
using ancientlib.game.entity.projectile;
using Microsoft.Xna.Framework;
using ancientlib.game.utils.chat;

namespace ancientlib.game.command
{
    class CommandSpawnEntity : ICommand
    {
        public void Execute(EntityPlayer sender, string[] args)
        {
            int id = 0;
            int.TryParse(args[0], out id);

            Entity entity = Entities.CreateEntityFromTypeID(id);

            if (entity == null)
            {
                sender.GetWorld().AddChatComponent(new ChatComponentText("No entity with id: " + id, Color.Red));
                return;
            }

            if (entity is EntityProjectile)
                ((EntityProjectile)entity).SetShooter(sender);

            entity.SetPosition(sender.GetPosition() + entity.GetHeight() * Vector3.Up);
            sender.GetWorld().SpawnEntity(entity);
        }

        public int GetMinArgs()
        {
            return 1;
        }
    }
}
