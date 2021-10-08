using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.world;

namespace ancientlib.game.utils.chat
{
    public class ChatComponentText : ChatComponent
    {
        private string text;

        public ChatComponentText()
        { }

        public ChatComponentText(string text, Color color)
        {
            this.text = text;
            this.color = color;
        }

        public ChatComponentText(string text) : this(text, Color.White)
        { }

        public override void Read(BinaryReader reader)
        {
            this.text = reader.ReadString();
            this.color = reader.ReadColorRGBA();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(text);
            writer.WriteColorRGBA(color);
        }

        public string GetText()
        {
            return this.text;
        }

        public void SetText(string text)
        {
            this.text = text;
        }
    }
}
