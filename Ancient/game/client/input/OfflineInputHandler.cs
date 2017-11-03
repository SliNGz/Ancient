using ancient.game.entity.player;
using ancientlib.game.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input
{
    public class OfflineInputHandler : IPlayerInput
    {
        private EntityPlayer player;

        public OfflineInputHandler()
        {
            this.player = Ancient.ancient.player;
        }

        public void Update() { }

        public void OnLeftButtonHeld()
        {
            if (player.GetItemInHand() != null && player.GetItemInHand().GetItem() is ItemWeapon)
                player.UseItemInHand();
        }

        public void OnLeftButtonPressed()
        {
            player.UseItemInHand();
        }

        public void OnLeftButtonReleased()
        { }

        public void OnRightButtonHeld()
        { }

        public void OnRightButtonPressed()
        {
            bool interacted = player.Interact();

            if (!interacted)
                player.UseItemInHandRightClick();
        }

        public void OnRightButtonReleased()
        { }
    }
}
