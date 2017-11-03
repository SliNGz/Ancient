using ancient.game.client.gui.component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input.utils
{
    public static class TextBoxUtils
    {
        public static void Delete(this GuiText guiText)
        {
            string text = guiText.GetText();

            if (text.Length == 0)
                return;

            guiText.SetText(text.Remove(text.Length - 1));
        }
        
        public static void Add(this GuiText guiText, char c)
        {
            if (!IsCharLegal(c))
                return;

            guiText.AddText(c);
        }

        public static bool IsCharLegal(char c)
        {
            return c >= ' ' && c <= 'z';
        }

        public static void AddNumber(this GuiText guiText, char c)
        {
            if (!IsCharNumber(c))
                return;

            guiText.AddText(c);
        }

        public static bool IsCharNumber(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
