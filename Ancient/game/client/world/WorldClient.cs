using ancient.game.client.gui;
using ancient.game.client.gui.component;
using ancient.game.client.network;
using ancient.game.client.particle;
using ancient.game.client.renderer.model;
using ancient.game.client.sound;
using ancient.game.client.world.chunk;
using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.input;
using ancient.game.renderers;
using ancient.game.renderers.model;
using ancient.game.renderers.world;
using ancient.game.world;
using ancient.game.world.chunk;
using ancientlib.game.entity;
using ancientlib.game.entity.projectile;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ancientlib.game.item;
using Microsoft.Xna.Framework.Media;
using ancientlib.game.world.biome;
using ancientlib.game.particle;

namespace ancient.game.client.world
{
    public class WorldClient : World
    {
        private WorldRenderer renderer;

        private Ancient ancient = Ancient.ancient;

        private bool isRemote;
        private NetPlay netPlay;

        private ParticleManager particleManager;

        private int bgmTicks;
        private bool stopBGM;

        public WorldClient(int seed, bool isRemote) : base(seed)
        {
            this.renderer = new WorldRenderer(this);
            this.chunkLoader = new ChunkLoaderClient(this);

            this.isRemote = isRemote;
            this.netPlay = new NetPlay(this);

            this.particleManager = new ParticleManager();
        }

        public override void Update(GameTime gameTime)
        {
            netPlay.Update();
            base.Update(gameTime);
            UpdateRainAnimation();
            UpdateDeathGui();
            particleManager.Update();
            UpdateBackgroundMusic();
        }

        public void Draw()
        {
            renderer.Draw();
        }

        public WorldRenderer GetRenderer()
        {
            return this.renderer;
        }

        public override void PlaySound(string name)
        {
            SoundManager.PlaySound(name);
        }

        public override void PlaySound(string name, float volume)
        {
            SoundManager.PlaySound(name, volume);
        }

        private void UpdateRainAnimation()
        {
            if (!isRaining)
                return;

            int x = rand.Next(-16, 16);
            int z = rand.Next(-16, 16);

            if (ancient.player.GetChunk() == null)
                return;

            float temp = ancient.player.GetTemperature();

            if (x * x + z * z <= 16 * 16)
            {
                //if (temp > 0 && temp < 30)
                ParticleSnow snow = new ParticleSnow(world);
                snow.SetPosition(ancient.player.GetPosition() + new Vector3(x, rand.Next(1, 7), z));
                snow.SetScale(Vector3.One * rand.Next(10, 25) / 1000F);
                SpawnParticle(snow);
                /*else if (temp <= 0)
                {
                    if (rainLastTicks % 4 == 0)
                        SpawnParticle(1, ancient.player.GetPosition() + new Vector3(x, 4, z));
                }*/
            }
        }

        public override void DisplayGui(string name)
        {
            ancient.guiManager.DisplayGui(GuiManager.GetGuiFromName(name));
        }

        public override void Display3DText(string text, Vector3 position, Vector3 velocity, Color color, float size = 3, int spacing = 1, float maxDistance = -1, int lifeSpan = 0, float sizeChange = 0, float hueChange = 0)
        {
            Gui ingame = Ancient.ancient.guiManager.ingame;
            ingame.AddComponent(
                new GuiText3D(ingame, text, position.X, position.Y, position.Z).SetVelocity(velocity).SetMaxDistance(maxDistance).SetLifeSpan(lifeSpan).
                SetSizeChange(sizeChange).SetHueChange(hueChange).SetColor(color).SetSize(size).SetSpacing(spacing).SetOutline(1)
                );
        }

        private void UpdateDeathGui()
        {
            if (!ancient.player.IsAlive() && ancient.guiManager.GetCurrentGui() != ancient.guiManager.death)
                ancient.guiManager.DisplayGui(ancient.guiManager.death);
        }

        public override bool IsRemote()
        {
            return this.isRemote;
        }

        public override EntityPlayer GetMyPlayer()
        {
            return ancient.player;
        }

        public NetPlay GetNetPlay()
        {
            return this.netPlay;
        }

        public override void OnPlayerChangeItemInHand(ItemStack hand)
        {
            base.OnPlayerChangeItemInHand(hand);
            Ancient.ancient.guiManager.ingame.OnPlayerChangeItemInHand(hand);
        }

        public ParticleManager GetParticleManager()
        {
            return this.particleManager;
        }

        public override void SpawnParticle(Particle particle)
        {
            particleManager.AddParticle(particle);
        }

        public override void DespawnParticle(Particle particle)
        {
            particleManager.RemoveParticle(particle);
        }

        private void UpdateBackgroundMusic()
        {
            Biome biome = ancient.player.GetBiome();
            List<string> bgmList = biome.GetBGMList();

            if (MediaPlayer.State == MediaState.Stopped)
            {
                if (bgmList.Count > 0)
                {
                    bgmTicks++;

                    if (bgmTicks == 512)
                    {
                        string bgm = bgmList[rand.Next(bgmList.Count)];
                        SoundManager.PlaySong(bgm);

                        bgmTicks = 0;
                    }
                }
            }
            else
            {
                if (!stopBGM)
                {
                    string activeBGM = MediaPlayer.Queue.ActiveSong.Name;

                    if (!bgmList.Contains(activeBGM))
                        stopBGM = true;
                }
            }

            UpdateBGMFadeOut();
        }

        private void UpdateBGMFadeOut()
        {
            if (stopBGM)
            {
                MediaPlayer.Volume -= 0.0025F;

                if (MediaPlayer.Volume == 0)
                {
                    stopBGM = false;
                    MediaPlayer.Stop();
                }
            }
        }
    }
}