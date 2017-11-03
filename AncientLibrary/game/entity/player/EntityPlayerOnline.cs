using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.network;
using Microsoft.Xna.Framework;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.item;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.network.packet.client.player;

namespace ancientlib.game.entity.player
{
    public class EntityPlayerOnline : EntityPlayer
    {
        public NetConnection netConnection;

        public Vector3 currentInput;

        public Vector3 lastPos;

        public UseAction useLeft = UseAction.RELEASE_LEFT;
        public UseAction useRight = UseAction.RELEASE_RIGHT;

        public EntityPlayerOnline(World world, NetConnection netConnection) : base(world)
        {
            this.netConnection = netConnection;
            this.netConnection.player = this;

            this.lastPos = GetPosition();
        }

        public override void Update(GameTime gameTime)
        {
            SetMovement(currentInput);

            if (isJumping)
                Jump();

            UpdateMouseInput();

            base.Update(gameTime);
        }

        private void UpdateMouseInput()
        {
            if (useLeft != UseAction.RELEASE_LEFT)
            {
                UseItemInHand();

                if (useLeft == UseAction.PRESS_LEFT)
                    useLeft = UseAction.RELEASE_LEFT;
            }

            if (useRight != UseAction.RELEASE_RIGHT)
            {
                bool interacted = Interact();

                if (!interacted)
                    UseItemInHandRightClick();
                else
                    useRight = UseAction.RELEASE_RIGHT;

                if (useRight == UseAction.PRESS_RIGHT)
                    useRight = UseAction.RELEASE_RIGHT;
            }
        }

        public override void Respawn()
        {
            base.Respawn();
            netConnection.SendPacket(new PacketPlayerRespawn());
        }

        public override bool AddItem(ItemStack itemStack)
        {
            bool itemAdded = base.AddItem(itemStack);

            if (itemAdded)
                netConnection.SendPacket(new PacketPlayerItemAction(itemStack, ItemAction.ADD_ITEM));

            return itemAdded;
        }

        public override bool RemoveItem(ItemStack itemStack)
        {
            bool itemRemoved = base.RemoveItem(itemStack);

            if (itemRemoved)
                netConnection.SendPacket(new PacketPlayerItemAction(itemStack, ItemAction.REMOVE_ITEM));

            return itemRemoved;
        }
    }
}
