using ancient.game.client.network;
using ancientlib.game.item;
using ancientlib.game.network.packet.client.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input
{
    public class OnlineInputHandler : IPlayerInput
    {
        public bool canSendInput;

        private UseAction lastUseLeft = UseAction.RELEASE_LEFT;
        private UseAction useLeft = UseAction.RELEASE_LEFT;

        private UseAction lastUseRight = UseAction.RELEASE_RIGHT;
        private UseAction useRight = UseAction.RELEASE_RIGHT;

        public void Update()
        {
            if (!canSendInput)
                return;

            if (useLeft != lastUseLeft)
            {
                NetClient.instance.SendPacket(new PacketPlayerUseItem(useLeft));
                lastUseLeft = useLeft;
            }

            if (useRight != lastUseRight)
            {
                NetClient.instance.SendPacket(new PacketPlayerUseItem(useRight));
                lastUseRight = useRight;
            }

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (lastUseLeft != UseAction.RELEASE_LEFT)
            {
                if (Ancient.ancient.player.GetItemInHand() != null && Ancient.ancient.player.GetItemInHand().CanUseItem(Ancient.ancient.player))
                {
                    Ancient.ancient.player.usingItemInHand = true;
                    Ancient.ancient.player.GetItemInHand().ticksElapsed = Ancient.ancient.player.GetItemInHand().GetCooldown(Ancient.ancient.player);
                }
            }
        }

        public void OnLeftButtonHeld()
        {
            if (Ancient.ancient.player.GetItemInHand() != null && Ancient.ancient.player.GetItemInHand().GetItem() is ItemWeapon)
                useLeft = UseAction.HOLD_LEFT;
        }

        public void OnLeftButtonPressed()
        {
            useLeft = UseAction.PRESS_LEFT;
        }

        public void OnLeftButtonReleased()
        {
            useLeft = UseAction.RELEASE_LEFT;
        }

        public void OnRightButtonHeld()
        {
            if (Ancient.ancient.player.GetItemInHand() != null && !(Ancient.ancient.player.GetItemInHand().GetItem() is ItemBlock))
                useRight = UseAction.HOLD_RIGHT;
        }

        public void OnRightButtonPressed()
        {
            useRight = UseAction.PRESS_RIGHT;
        }

        public void OnRightButtonReleased()
        {
            useRight = UseAction.RELEASE_RIGHT;
        }
    }
}
