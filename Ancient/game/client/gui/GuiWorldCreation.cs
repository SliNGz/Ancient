using ancient.game.client.world;
using ancient.game.entity.player;
using ancient.game.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui
{
    public class GuiWorldCreation : GuiMenuBackground
    {
        public GuiWorldCreation(GuiManager guiManager) : base(guiManager, "world_creation")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.playMenu;
        }

        public override void OnDisplay(Gui lastGui)
        {
            base.OnDisplay(lastGui);
            Ancient.ancient.CreateWorld(15050, false);
            Ancient.ancient.SpawnPlayer();
            guiManager.DisplayGui(guiManager.ingame);
        }
    }
}
