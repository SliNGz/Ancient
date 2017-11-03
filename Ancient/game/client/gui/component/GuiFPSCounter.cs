using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ancient.game.client.gui.component
{
    class GuiFPSCounter : GuiText
    {
        private double elapsedTime;
        private int frameRate = 0;
        private int frameCounter = 0;

        public GuiFPSCounter(string text) : base(text)
        { }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
          //  elapsedTime += Game.game.gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= 1000)
            {
                elapsedTime = 0;
                frameRate = frameCounter;
                frameCounter = 0;
            }

            frameCounter++;

            this.text = "FPS: " + frameRate;

            base.Draw(spriteBatch);
        }
    }
}
