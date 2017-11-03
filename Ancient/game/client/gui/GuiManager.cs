using ancient.game.client.gui.component;
using ancient.game.client.renderer.texture;
using ancient.game.client.sound;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ancient.game.client.gui
{
    public class GuiManager
    {
        public static readonly Dictionary<string, Gui> guis = new Dictionary<string, Gui>();

        private Gui currentGui;
        private Gui lastGui;

        private GuiCursor cursor;

        public GuiIngame ingame;
        public GuiIngameMenu menu;
        public GuiOptions options;
        public GuiVideoSettings videoSettings;
        public GuiInventory inventory;
        public GuiDeath death;
        public GuiMap map;
        public GuiChat chat;
        public GuiMainMenu mainMenu;
        public GuiCharacterCreation characterCreation;
        public GuiServerBrowser serverBrowser;
        public GuiPlayMenu playMenu;
        public GuiWorldCreation worldCreation;
        public GuiDebug debug;

        private float backgroundHue;
        public Color backgroundColor;

        public GuiManager()
        {
            this.ingame = new GuiIngame(this);
            this.menu = new GuiIngameMenu(this);
            this.options = new GuiOptions(this);
            this.videoSettings = new GuiVideoSettings(this);
            this.inventory = new GuiInventory(this);
            this.death = new GuiDeath(this);
            this.map = new GuiMap(this);
            this.chat = new GuiChat(this);
            this.mainMenu = new GuiMainMenu(this);
            this.characterCreation = new GuiCharacterCreation(this);
            this.serverBrowser = new GuiServerBrowser(this);
            this.playMenu = new GuiPlayMenu(this);
            this.worldCreation = new GuiWorldCreation(this);
            this.debug = new GuiDebug(this);

            this.currentGui = mainMenu;
            this.lastGui = currentGui;

            this.backgroundColor = Utils.HSVToRGB(backgroundHue, 0.35F, 1);
        }

        public void Initialize()
        {
            foreach (Gui gui in guis.Values)
                gui.Initialize();

            this.cursor = new GuiCursor();
        }

        public void Update(MouseState mouseState)
        {
            this.backgroundHue += 0.025F;
            this.backgroundColor = Utils.HSVToRGB(backgroundHue, 0.35F, 1);

            if (currentGui.IsCursorVisible())
                cursor.Update(mouseState);

            if (currentGui != ingame)
                ingame.Update(mouseState);

            currentGui.Update(mouseState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Ancient.ancient.world != null)
            {
                if (!currentGui.ShouldDrawWorldBehind())
                    Ancient.ancient.world.GetRenderer().ResetGraphics(currentGui.GetBackgroundColor());
            }
            else
                Ancient.ancient.device.Clear(currentGui.GetBackgroundColor());

            currentGui.Draw3D();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (currentGui != ingame && currentGui.ShouldDrawWorldBehind())
                ingame.Draw(spriteBatch);

            currentGui.Draw(spriteBatch);
            DrawCursor(spriteBatch);

            spriteBatch.End();
        }

        private void DrawCursor(SpriteBatch spriteBatch)
        {
            if (currentGui.IsCursorVisible())
                cursor.Draw(spriteBatch);
        }

        public void DisplayGui(Gui gui)
        {
            this.currentGui.OnClose();
            this.lastGui = currentGui;
            this.currentGui = gui;
            this.currentGui.OnDisplay(lastGui);
        }

        public Gui GetCurrentGui()
        {
            return this.currentGui;
        }

        public void DisplayLastGui()
        {
            if (this.currentGui.GetLastGui() != null)
            {
                DisplayGui(this.currentGui.GetLastGui());
                SoundManager.PlaySound("cancel");
            }
        }

        public static Gui GetGuiFromName(string name)
        {
            Gui gui = null;
            guis.TryGetValue(name, out gui);
            return gui;
        }

        public Gui GetLastGui()
        {
            return this.lastGui;
        }
    }
}
