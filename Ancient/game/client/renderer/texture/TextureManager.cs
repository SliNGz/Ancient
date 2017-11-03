using ancient.game.client.renderer.model;
using ancient.game.renderers.model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.texture
{
    class TextureManager
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static readonly string basePath = "texture/";

        public static void LoadContent(ContentManager content)
        {
            LoadTexture(content, "gui/crosshair");
            LoadTexture(content, "gui/font");

            LoadTexture(content, "gui/component/cursor");
            LoadTexture(content, "gui/component/button/button");
            LoadTexture(content, "gui/component/button/button2");

            LoadTexture(content, "gui/player/bar_frame");
            LoadTexture(content, "gui/player/health_bar");
            LoadTexture(content, "gui/player/mana_bar");
            LoadTexture(content, "gui/player/exp_bar");
            LoadTexture(content, "gui/player/use_bar");
            LoadTexture(content, "gui/player/inventory_bar");

            LoadTexture(content, "gui/inventory/inventory_window");
            LoadTexture(content, "gui/inventory/inventory_slots");
        }

        private static void LoadTexture(ContentManager content, string path)
        {
            string[] splitedPath = path.Split('/');
            textures.Add(splitedPath[splitedPath.Length - 1], content.Load<Texture2D>(basePath + path));
        }

        public static Texture2D GetTextureFromName(string name)
        {
            Texture2D texture = null;
            textures.TryGetValue(name, out texture);

            return texture;
        }
    }
}
