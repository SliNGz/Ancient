using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.settings
{
    public class ControlsSettings
    {
        private VideoSettings videoSettings;
        // Video Settings
        private int antiAliasing;
        private int renderDistance;

        // Audio Settings
        private float volume;

        // Gameplay Settings
        private float sensitivity;

        public ControlsSettings()
        {
            this.antiAliasing = 8;
            this.renderDistance = 18;

            this.volume = 0.2F;

            this.sensitivity = 1;
        }

        public int GetAntiAliasing()
        {
            return this.antiAliasing;
        }

        public void SetAntiAliasing(int antiAliasing)
        {
            this.antiAliasing = MathHelper.Clamp(antiAliasing, 0, 16);
        }

        public int GetRenderDistance()
        {
            return this.renderDistance;
        }

        public void SetRenderDistance(int renderDistance)
        {
            this.renderDistance = MathHelper.Clamp(renderDistance, 0, 16);
        }

        public float GetVolume()
        {
            return this.volume;
        }

        public void SetVolume(float volume)
        {
            this.volume = MathHelper.Clamp(volume, 0, 1);
            MediaPlayer.Volume = volume;
        }

        public void AddVolume(float add)
        {
            SetVolume(volume + add);
        }

        public float GetSensitivity()
        {
            return this.sensitivity;
        }

        public void SetSensitivity(float sensitivity)
        {
            this.sensitivity = sensitivity;
        }
    }
}
