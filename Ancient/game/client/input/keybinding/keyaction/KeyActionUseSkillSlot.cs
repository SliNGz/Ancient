using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionUseSkillSlot : IKeyAction
    {
        private int slot;

        public KeyActionUseSkillSlot(int slot)
        {
            this.slot = slot;
        }

        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            player.ActivateSkill(this.slot);
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
