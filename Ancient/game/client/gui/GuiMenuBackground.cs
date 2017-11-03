using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ancient.game.client.gui
{
    public class GuiMenuBackground : Gui
    {
        public GuiMenuBackground(GuiManager guiManager, string name) : base(guiManager, name)
        {
            this.drawWorldBehind = false;
            this.backgroundColor = guiManager.backgroundColor;
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            this.backgroundColor = guiManager.backgroundColor;
        }
    }
}
