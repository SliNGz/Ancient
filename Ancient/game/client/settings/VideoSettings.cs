using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ancient.game.client.settings
{
    public class VideoSettings : Settings
    {
        public static string FILE_NAME = "videoSettings";

        public static int MIN_RENDER_DISTANCE = 2;
        public static int MAX_RENDER_DISTANCE = 16;

        public static int MAX_ANTI_ALIASING = 16;

        private int renderDistance;
        private int antiAliasing;

        public VideoSettings() : base(FILE_NAME)
        { }

        protected override void InitializeDefaultSettingsValues()
        {
            this.renderDistance = MIN_RENDER_DISTANCE;
            this.antiAliasing = 0;
        }

        protected override void LoadSettings(StreamReader reader)
        {
            SetRenderDistance(ReadIntFromLine(reader.ReadLine()));
            SetAntiAliasing(ReadIntFromLine(reader.ReadLine()));
        }

        protected override void SaveSettings(StreamWriter writer)
        {
            writer.WriteLine("renderdistance:" + renderDistance);
            writer.WriteLine("antialiasing:" + antiAliasing);
        }

        public int GetAntiAliasing()
        {
            return this.antiAliasing;
        }

        public void SetAntiAliasing(int antiAliasing)
        {
            this.antiAliasing = MathHelper.Clamp(antiAliasing, 0, MAX_ANTI_ALIASING);
        }

        public void AddAntiAliasing(int add)
        {
            SetAntiAliasing(this.antiAliasing + add);
        }

        public int GetRenderDistance()
        {
            return this.renderDistance;
        }

        public void SetRenderDistance(int renderDistance)
        {
            this.renderDistance = MathHelper.Clamp(renderDistance, MIN_RENDER_DISTANCE, MAX_RENDER_DISTANCE);

            if (Ancient.ancient != null && Ancient.ancient.player != null)
                Ancient.ancient.player.SetRenderDistance(this.renderDistance);
        }

        public void AddRenderDistance(int add)
        {
            SetRenderDistance(this.renderDistance + add);
        }
    }
}
