using ancient.game.client.gui.component;
using ancient.game.client.renderer.texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.font
{
    class FontRenderer
    {
        private static Dictionary<char, int> characterWidth = new Dictionary<char, int>();
        public static Texture2D font;
        public static int fontSize = 8;

        public static void Initialize()
        {
            font = TextureManager.GetTextureFromName("font");

            Color[] data = new Color[fontSize * fontSize];

            for (char character = ' '; character <= 'z'; character++)
            {
                int index = character - ' ';
                int x = (index % 16) * fontSize;
                int y = (index / 16) * fontSize;

                font.GetData(0, new Rectangle(x, y, fontSize, fontSize), data, 0, fontSize * fontSize);

                int width = 0;

                if (character == ' ')
                {
                    characterWidth.Add(character, fontSize / 2);
                    continue;
                }

                for (int i = 0; i < fontSize; i++)
                {
                    for (int j = 0; j < fontSize; j++)
                    {
                        if (data[i + j * fontSize] != Color.Transparent)
                        {
                            width = i + 1;
                            j = fontSize;
                        }
                    }
                }

                characterWidth.Add(character, width);
            }
        }

        public static int GetWidthOfCharacter(char character)
        {
            int width = 0;

            if (characterWidth.TryGetValue(character, out width))
                return width;

            return -1;
        }

        public static void DrawString(SpriteBatch spriteBatch, string text, float x, float y, Color color, float size, int spacing, int outline, Color outlineColor)
        {
            float drawX = x;

            size = MathHelper.Clamp(size, 0.75F, 64);

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                int index = character - ' ';
                int rectX = (index % 16) * fontSize;
                int rectY = (index / 16) * fontSize;

                int width = GetWidthOfCharacter(character);
                if (width == -1)
                {
                    drawX += fontSize * size + spacing;
                    continue;
                }

                if (outline > 0)
                {
                    spriteBatch.Draw(font, new Vector2((int)drawX - outline, (int)y), new Rectangle(rectX, rectY, fontSize, fontSize), outlineColor, 0, Vector2.Zero, size, SpriteEffects.None, 0);
                    spriteBatch.Draw(font, new Vector2((int)drawX + outline, (int)y), new Rectangle(rectX, rectY, fontSize, fontSize), outlineColor, 0, Vector2.Zero, size, SpriteEffects.None, 0);
                    spriteBatch.Draw(font, new Vector2((int)drawX, (int)y - outline), new Rectangle(rectX, rectY, fontSize, fontSize), outlineColor, 0, Vector2.Zero, size, SpriteEffects.None, 0);
                    spriteBatch.Draw(font, new Vector2((int)drawX, (int)y + outline), new Rectangle(rectX, rectY, fontSize, fontSize), outlineColor, 0, Vector2.Zero, size, SpriteEffects.None, 0);
                }

                spriteBatch.Draw(font, new Vector2((int)drawX, (int)y), new Rectangle(rectX, rectY, fontSize, fontSize), color, 0, Vector2.Zero, size, SpriteEffects.None, 0);

                drawX += width * size + spacing;
            }
        }

        public static float MeasureGuiText(GuiText guiText)
        {
            string text = guiText.GetText();
            float size = guiText.GetSize();
            int spacing = guiText.GetSpacing();
            float width = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                int index = character - '0';
                int rectX = (index % 16) * fontSize;
                int rectY = (index / 16) * fontSize;

                int charWidth = GetWidthOfCharacter(character);
                if (charWidth == -1)
                {
                    Console.WriteLine("FontRenderer - Couldn't draw the char: " + character);
                    continue;
                }

                width += charWidth * size + spacing;
            }

            return width;
        }

        public static float MeasureGuiText(string text, float size, int spacing)
        {
            float width = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                int index = character - '0';
                int rectX = (index % 16) * fontSize;
                int rectY = (index / 16) * fontSize;

                int charWidth = GetWidthOfCharacter(character);
                if (charWidth == -1)
                {
                    Console.WriteLine("FontRenderer - Couldn't draw the char: " + character);
                    continue;
                }

                width += charWidth * size + spacing;
            }

            return width;
        }
    }
}
