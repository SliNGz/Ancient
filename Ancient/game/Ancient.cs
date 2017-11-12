using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ancient.game.client.world;
using ancientlib.game.init;
using ancient.game.client.input.keybinding;
using ancient.game.client.renderer.model;
using ancient.game.client.settings;
using ancient.game.entity.player;
using ancient.game.client.sound;
using ancient.game.client.gui;
using ancient.game.client.renderer.texture;
using ancient.game.client.renderer.font;
using ancient.game.client.input.utils;
using ancient.game.input;
using ancient.game.world;
using ancient.game.entity;
using ancient.game.client.network;
using ancient.game.client.network.packet.handler;
using System.Threading;
using ancientlib.game.utils;
using ancientlib.game.log;
using System.Collections.Generic;
using ancient.game.utils;
using ancientlib.game.network.packet.common.status;
using ancient.game.client.particle;
using ancient.game.client.renderer.entity;

namespace ancient.game
{
    public class Ancient : Microsoft.Xna.Framework.Game
    {
        public static Ancient ancient;

        public GraphicsDeviceManager graphics;
        public GraphicsDevice device;

        public SpriteBatch spriteBatch;

        private TimeSpan deltaTime;
        public GameTime gameTime;

        private TimeSpan currentTime;
        private TimeSpan accumulator;

        public const int tickRate = 128;

        public WorldClient world;

        public EntityPlayer player;
        public PlayerInputManager inputManager;

        public NetClient netClient;

        public MouseState mouseState;
        public MouseState oldMouseState;

        public KeyboardState keyState;
        public KeyboardState oldKeyState;

        private double elapsedTime;
        public int frameRate = 0;
        private int frameCounter = 0;

        public Vector2 screenCenter;
        public GameSettings gameSettings;

        public GuiManager guiManager;

        public Effect effect;

        public KeyboardDispatcher keyDispatcher;

        public bool worldLoaded;

        public Ancient()
        {
            this.gameSettings = new GameSettings();

            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            graphics.PreparingDeviceSettings += (s, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = gameSettings.GetAntiAliasing();
            };

            Content.RootDirectory = "Content";

            ancient = this;
        }

        protected override void Initialize()
        {
            try
            {
                device = GraphicsDevice;

                this.IsFixedTimeStep = false;
                this.IsMouseVisible = false;

                this.deltaTime = TimeSpan.FromSeconds(1.0 / tickRate);
                this.gameTime = new GameTime(TimeSpan.Zero, deltaTime);

                this.keyDispatcher = new KeyboardDispatcher(Window);

                this.player = new EntityPlayer(null);

                this.guiManager = new GuiManager();

                this.inputManager = new PlayerInputManager();

                effect = Content.Load<Effect>("effect");

                InitializeGame();

                this.netClient = new NetClient();

                this.screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

                base.Initialize();
            }
            catch (Exception ex)
            {
                CrashReport.CreateCrashReport(ex);
                Exit();
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.LoadContent(Content);
            FontRenderer.Initialize();
            SoundManager.LoadContent(Content);

            guiManager.Initialize();
        }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                TimeSpan newTime = gameTime.TotalGameTime;
                TimeSpan frameTime = newTime - currentTime;
                currentTime = newTime;

                accumulator += frameTime;

                while (accumulator >= deltaTime)
                {
                    DoUpdate();

                    accumulator -= deltaTime;
                    this.gameTime.TotalGameTime += deltaTime;
                }

                DoDraw(gameTime);

                base.Update(gameTime);
            }
            catch (Exception ex)
            {
                CrashReport.CreateCrashReport(ex);
                Exit();
            }
        }

        private void DoUpdate()
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            inputManager.Update(gameTime);

            netClient.Update();

            if (world != null)
                world.Update(gameTime);

            guiManager.Update(mouseState);

            oldMouseState = mouseState;
            oldKeyState = keyState;
        }

        private void DoDraw(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= 1000)
            {
                elapsedTime = 0;
                frameRate = frameCounter;
                frameCounter = 0;
            }

            frameCounter++;

            if (world != null)
                world.Draw();

            guiManager.Draw(spriteBatch);
        }

        private void InitializeGame()
        {
            Init.Initialize();
            ModelDatabase.Initialize();
            KeyBindings.Initialize();
            ClientPacketHandlers.Initialize();
            EntityRenderers.Initialize();
        }

        public void ToggleFullscreen()
        {
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 480;
            }
            else
            {
                graphics.PreferredBackBufferWidth = device.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = device.DisplayMode.Height;
            }

            graphics.ApplyChanges();
            graphics.ToggleFullScreen();

            this.screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            this.guiManager.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs e)
        {
            OnLeaveWorld();
            base.OnExiting(sender, e);
        }

        public bool IsIngame()
        {
            return guiManager.GetCurrentGui() == guiManager.ingame || guiManager.GetCurrentGui() == guiManager.debug;
        }

        public void CreateWorld(int seed, bool isRemote)
        {
            WorldClient world = new WorldClient(seed, isRemote);
            this.player.SetWorld(world);
            this.world = world;
        }

        public void SpawnPlayer()
        {
            world.SpawnEntity(player);
        }

        public void OnLeaveWorld()
        {
            if (world.IsRemote())
            {
                netClient.SendPacket(new PacketDisconnect());
                netClient.GetNetConnection().SendPackets(); // Flush all outgoing packets (No more updates will be called after exiting - no packet sending).

                inputManager.onlineInput.canSendInput = false;
                inputManager.playerInput = inputManager.offlineInput;
            }

            Entity.nextEntityID = 0;
            ThreadUtils.Clear();
            world = null;

            player.SetRotation(0, 0, 0);
            player.SetHeadRotation(0, 0);
        }
    }
}