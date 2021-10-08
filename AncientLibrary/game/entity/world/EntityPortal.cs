using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.model;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;
using ancientlib.game.world;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.utils;

namespace ancientlib.game.entity.world
{
    public class EntityPortal : Entity
    {
        private EntityPortal connectedPortal;

        public EntityPortal(World world) : base(world)
        {
            this.interactWithBlocks = false;
            this.interactWithEntities = false;
            this.gravity = 0;
        }

        public override void OnPlayerInteraction(EntityPlayer player)
        {
            base.OnPlayerInteraction(player);

            if (this.connectedPortal == null)
                CreateNewConnectedPortal();

            player.Teleport(connectedPortal.GetPosition());
        }

        private void CreateNewConnectedPortal()
        {
            this.connectedPortal = new EntityPortal(world);

            int x = (int)this.x + world.rand.Next(500, 2000) * world.rand.NextSign();
            int z = (int)this.z + world.rand.Next(500, 2000) * world.rand.NextSign();

            this.connectedPortal.SetPosition(x, y, z);
            this.connectedPortal.SetConnectedPortal(this);

            world.SpawnEntity(connectedPortal);
        }

        public void SetConnectedPortal(EntityPortal connectedPortal)
        {
            this.connectedPortal = connectedPortal;
        }

        public EntityPortal GetConnectedPortal()
        {
            return this.connectedPortal;
        }

        public override string GetEntityName()
        {
            return "portal";
        }

        public override string GetRendererName()
        {
            return "portal";
        }

        public override EntityModelCollection GetModelCollection()
        {
            return EntityModelCollection.portal;
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.25F, 0.25F, 0.25F);
        }
    }
}
