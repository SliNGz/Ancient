using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.sound
{
    class SoundManager
    {
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Song> music = new Dictionary<string, Song>();

        public static readonly string soundBasePath = "sound/";
        public static readonly string musicBasePath = "music/";

        public static ContentManager content;

        public static void LoadContent(ContentManager content)
        {
            SoundManager.content = content;
            /*  Sounds  */

            //Character
            LoadSound("character/jump");
            LoadSound("character/level_up");

            //GUI
            LoadSound("gui/select");
            LoadSound("gui/cancel");

            //Item
            LoadSound("item/pickup_item");

            LoadSound("item/bow/shoot_arrow_0");
            LoadSound("item/bow/shoot_arrow_1");
            LoadSound("item/bow/shoot_arrow_2");
            LoadSound("item/bow/shoot_arrow_3");

            //Skill
            LoadSound("skill/thief/flash_jump");

            //World
            LoadSound("world/explosion_0");

            /*  Songs   */

            LoadSong("bgm/bgm_snow_0");
            LoadSong("bgm/logo");
        }

        private static void LoadSound(string path)
        {
            string[] splitedPath = path.Split('/');
            sounds.Add(splitedPath[splitedPath.Length - 1], content.Load<SoundEffect>(soundBasePath + path));
        }

        private static SoundEffect GetSoundFromName(string name)
        {
            SoundEffect sound = null;
            sounds.TryGetValue(name, out sound);

            return sound;
        }

        public static void PlaySound(string name)
        {
            PlaySound(name, Ancient.ancient.gameSettings.GetVolume());
        }

        public static void PlaySound(string name, float volume)
        {
            if (Ancient.ancient.gameSettings.GetVolume() == 0)
                return;

            if (name == null)
                return;

            SoundEffect sound = GetSoundFromName(name);

            if (sound == null)
                throw new NullReferenceException("SoundManager - Couldn't find the sound: " + name);

            sound.Play(volume, 0, 0);
        }

        private static void LoadSong(string path)
        {
            string[] splitedPath = path.Split('/');
            music.Add(splitedPath[splitedPath.Length - 1], content.Load<Song>(musicBasePath + path));
        }

        private static Song GetSongFromName(string name)
        {
            Song song = null;
            music.TryGetValue(name, out song);

            return song;
        }

        public static void PlaySong(string name)
        {
            PlaySong(name, Ancient.ancient.gameSettings.GetVolume());
        }

        public static void PlaySong(string name, float volume)
        {
            if (name == null)
                return;

            Song song = GetSongFromName(name);

            if (song == null)
                throw new NullReferenceException("SoundManager - Couldn't find the song: " + name);

            MediaPlayer.Volume = volume;
            MediaPlayer.Play(song);
        }
    }
}
