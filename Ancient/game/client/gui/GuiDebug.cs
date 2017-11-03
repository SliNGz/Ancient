using ancient.game.client.gui.component;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancient.game.world.chunk;
using ancientlib.game.world.biome;
using ancient.game.entity.player;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.client.utils;

namespace ancient.game.client.gui
{
    public class GuiDebug : Gui
    {
        private EntityPlayer player;

        private GuiText position;
        private GuiText temperatureRainfall;
        private GuiText chunksRendered;

        private GuiText fps;

        public GuiDebug(GuiManager guiManager) : base(guiManager, "debug")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;

            this.drawWorldBehind = true;
            this.isCursorVisible = false;
            this.player = Ancient.ancient.player;

            this.position = new GuiText("Position: ");
            this.position.SetX(GuiUtils.GetRelativeXFromX(3));
            this.position.SetY(GuiUtils.GetRelativeYFromY(3));
            this.position.SetOutline(1);
            this.components.Add(position);

            this.temperatureRainfall = new GuiText("Temperature: ").SetSize(3);
            this.temperatureRainfall.SetX(position.GetX());
            this.temperatureRainfall.SetY(position.GetY() + GuiUtils.GetRelativeYFromY(position.GetHeight() + 10));
            this.temperatureRainfall.SetOutline(1);
            this.components.Add(temperatureRainfall);

            this.chunksRendered = new GuiText("Chunks Rendered: ");
            this.chunksRendered.SetX(position.GetX());
            this.chunksRendered.SetY(temperatureRainfall.GetY() + GuiUtils.GetRelativeYFromY(temperatureRainfall.GetHeight() + 10));
            this.chunksRendered.SetOutline(1);
            this.components.Add(chunksRendered);

            this.fps = new GuiText("FPS: ");
            this.fps.SetX(position.GetX());
            this.fps.SetY(chunksRendered.GetY() + GuiUtils.GetRelativeYFromY(chunksRendered.GetHeight() + 30));
            this.fps.SetOutline(1);
            this.fps.SetColor(Color.Yellow);
            this.components.Add(fps);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            this.position.SetText("Position: " + player.GetPosition());

            this.chunksRendered.SetText("Chunks Rendered: " + Ancient.ancient.world.GetRenderer().GetChunkRenderer().renderList.Count);

            Chunk chunk = player.GetChunk();

            if (chunk != null)
                this.temperatureRainfall.SetText("Temperature: " + BiomeManager.GetTemperature((int)player.GetX(), (int)player.GetZ())
                    + ", Rainfall: " + BiomeManager.GetRainfall((int)player.GetX(), (int)player.GetZ()));

            this.fps.SetText("FPS: " + Ancient.ancient.frameRate);
        }
    }
}
