using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.settings
{
    public class GameSettings
    {
        private ControlsSettings controlsSettings;
        private VideoSettings videoSettings;
        private AudioSettings audioSettings;

        public GameSettings()
        {
            this.controlsSettings = new ControlsSettings();
            this.videoSettings = new VideoSettings();
            this.audioSettings = new AudioSettings();
        }

        public ControlsSettings GetControlsSettings()
        {
            return this.controlsSettings;
        }

        public VideoSettings GetVideoSettings()
        {
            return this.videoSettings;
        }

        public AudioSettings GetAudioSettings()
        {
            return this.audioSettings;
        }
    }
}
