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

        public static ContentManager content;

        public static void LoadContent(ContentManager content)
        {
            TextureManager.content = content;
            LoadTexture("gui/crosshair");
            LoadTexture("gui/font");

            LoadTexture("gui/component/cursor");
            LoadTexture("gui/component/button/button");
            LoadTexture("gui/component/button/button2");
            LoadTexture("gui/component/scroll");
            LoadTexture("gui/component/scroll_arrow");

            LoadTexture("gui/player/bar_frame");
            LoadTexture("gui/player/exp_frame");
            LoadTexture("gui/player/health_bar");
            LoadTexture("gui/player/mana_bar");
            LoadTexture("gui/player/exp_bar");
            LoadTexture("gui/player/use_bar");

            LoadTexture("gui/inventory/inventory_window");
            LoadTexture("gui/inventory/inventory_slots");
            LoadTexture("gui/inventory/inventory_slot");
            LoadTexture("gui/inventory/inventory_bar");
            LoadTexture("gui/inventory/selected_slot");
        }

        private static void LoadTexture(string path)
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
