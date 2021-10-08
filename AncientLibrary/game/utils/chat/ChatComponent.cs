using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils.chat
{
    public abstract class ChatComponent
    {
        public static float MAX_ALPHA = 3;
        public static float FADE_AMOUNT = 1 / 256F;

        protected Color color;
        public float alpha;

        public ChatComponent()
        {
            this.color = Color.White;
            this.alpha = MAX_ALPHA;
        }

        public abstract void Read(BinaryReader reader);

        public abstract void Write(BinaryWriter writer);

        public virtual void Update()
        {
            alpha = MathHelper.Clamp(alpha - FADE_AMOUNT, 0, MAX_ALPHA);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public Color GetColor()
        {
            return this.color * alpha;
        }
    }
}
