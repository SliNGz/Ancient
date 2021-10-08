using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionFlyDown : IKeyAction
    {
        private bool updatedModel;

        public void UpdateHeld(EntityPlayer player)
        {
            UpdatePressed(player);
        }

        public void UpdatePressed(EntityPlayer player)
        {
            if (player.HasNoClip())
                player.inputVector += Vector3.Down;
            else
            {
                if (player.IsRiding())
                {
                    player.GetMount().SetModel(player.GetMount().GetModelCollection().GetSleepingModel());
                    updatedModel = true;
                }
                else if (player.GetModel() != player.GetModelCollection().GetSittingModel())
                {
                    player.SetModel(player.GetModelCollection().GetSittingModel());
                    updatedModel = true;
                }
            }
        }

        public void UpdateReleased(EntityPlayer player)
        {
            if (updatedModel)
            {
                if (player.IsRiding())
                    player.GetMount().SetModel(player.GetMount().GetModelCollection().GetStandingModel());
                else
                    player.SetModel(player.GetModelCollection().GetStandingModel());

                updatedModel = false;
            }
        }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
