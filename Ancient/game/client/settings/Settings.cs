using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.settings
{
    public abstract class Settings
    {
        public static string SETTINGS_PATH = Environment.CurrentDirectory + "/settings/";
        private string fileName;
        private string path;

        public Settings(string fileName)
        {
            this.fileName = fileName;
            Initialize();
        }

        private void Initialize()
        {
            InitializeSettingsDirectory();
            InitializeDefaultSettingsValues();

            this.path = SETTINGS_PATH + fileName + ".cfg";

            if (File.Exists(path))
            {
                StreamReader reader = File.OpenText(path);
                LoadSettings(reader);
                reader.Close();
            }
            else
                SaveSettings(); // Saves the default settings initialized above.
        }

        private void InitializeSettingsDirectory()
        {
            if (!Directory.Exists(SETTINGS_PATH))
                Directory.CreateDirectory(SETTINGS_PATH);
        }

        protected abstract void LoadSettings(StreamReader reader);

        protected abstract void InitializeDefaultSettingsValues();

        protected abstract void SaveSettings(StreamWriter writer);

        public void SaveSettings()
        {
            StreamWriter writer = File.CreateText(path);
            SaveSettings(writer);
            writer.Close();
        }

        protected string ReadValueFromLine(string str)
        {
            string[] arr = str.Split(':');
            return arr[1];
        }

        protected int ReadIntFromLine(string str)
        {
            str = ReadValueFromLine(str);
            return int.Parse(str);
        }
    }
}
