using ancient.game.client.gui.component;
using ancient.game.client.gui.component.button;
using ancient.game.client.network;
using ancientlib.game.network.packet.common.player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui
{
    public class GuiDeath : Gui
    {
        private static string[] deathTexts = new string[] { "Boo hoo, you died!", "Try again next time!", "You suck.", "lol.", "This ain't no game." };

        private GuiText deathText;
        private GuiButton respawnButton;

        public GuiDeath(GuiManager guiManager) : base(guiManager, "death")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.deathText = new GuiText("Boo hoo, You died!", 5, 2).SetColor(Color.DarkRed).SetOutline(1).SetSpacing(3);
            this.deathText.Centralize();
            this.deathText.AddY(-0.2F);
            this.components.Add(deathText);

            this.respawnButton = new GuiButtonText(new GuiText("Respawn").SetSpacing(2).SetOutline(1));
            this.respawnButton.ButtonClickEvent += new ButtonClickEventHandler(OnRespawnButtonClicked);
            this.respawnButton.Centralize();
            this.respawnButton.CentralizeText();
            this.components.Add(respawnButton);
        }

        public override void OnDisplay(Gui lastGui)
        {
            base.OnDisplay(lastGui);
            this.deathText.SetText(deathTexts[Ancient.ancient.world.rand.Next(deathTexts.Length)]);
            this.deathText.Centralize();
            this.deathText.AddY(-0.2F);
        }

        private void OnRespawnButtonClicked(object sender, EventArgs e)
        {
            if (Ancient.ancient.world.IsRemote())
                NetClient.instance.SendPacket(new PacketPlayerRespawn());
            else
            {
                Ancient.ancient.player.Respawn();
                guiManager.DisplayGui(guiManager.ingame);
            }
        }
    }
}
