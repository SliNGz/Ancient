using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.player;
using ancient.game.entity.player;

namespace ancientlib.game.entity.npc
{
    class EntityNPC : EntityPlayerBase
    {
        public EntityNPC(World world) : base(world)
        {
            this.name = "NPC";
        }

        public override string GetEntityName()
        {
            return "npc";
        }

        public override void OnPlayerInteraction(EntityPlayer player)
        {
            base.OnPlayerInteraction(player);

            world.Display3DText("Hello world", GetPosition() + new Vector3(0, 1, 0), Vector3.Zero, Color.White);
        }
    }
}
