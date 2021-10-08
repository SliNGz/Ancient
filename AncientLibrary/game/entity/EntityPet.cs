using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancient.game.world;
using ancientlib.game.entity.ai;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;
using ancientlib.game.item;
using ancientlib.game.world;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.player;

namespace ancientlib.game.entity
{
    public abstract class EntityPet : EntityDeveloping
    {
        protected Color nameColor;
        protected EntityLiving owner;

        protected Item food;

        public EntityPet(World world) : base(world)
        {
            this.name = "Nameless";
            this.nameColor = Color.White;
            this.owner = null;
        }

        public Color GetNameColor()
        {
            return this.nameColor;
        }

        public void SetNameColor(Color nameColor)
        {
            this.nameColor = nameColor;
        }

        public EntityLiving GetOwner()
        {
            return this.owner;
        }

        public void SetOwner(EntityLiving owner)
        {
            this.owner = owner;
        }

        public bool HasOwner()
        {
            return this.owner != null;
        }

        public Item GetFood()
        {
            return this.food;
        }

        public void SetFood(Item food)
        {
            this.food = food;
        }

        protected override bool CanCollideWithEntityBoundingBox(Entity entity)
        {
            if (entity == owner)
                return false;

            if (entity is EntityPet)
            {
                if (((EntityPet)entity).owner == owner)
                    return false;
            }

            return true;
        }

        public override bool IsAlive()
        {
            if (HasOwner())
                return this.health > 0;

            return base.IsAlive();
        }

        public override void OnPlayerInteraction(EntityPlayer player)
        {
            base.OnPlayerInteraction(player);

            if (!HasOwner())
            {
                if (player.GetItemInHand() == null)
                    return;

                if (player.GetItemInHand().GetItem() == food)
                {
                    player.AddPet(this);

                    if (world is WorldServer)
                        ((EntityPlayerOnline)player).netConnection.SendPacket(new PacketPlayerTamePet(this));
                }
            }
        }

        protected override void UpdateFallDamage()
        {
            if (HasOwner())
                return;

            base.UpdateFallDamage();
        }
    }
}
