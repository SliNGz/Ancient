using ancient.game.client.renderer.texture;
using ancient.game.client.sound;
using ancient.game.client.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui.component.button
{
    public delegate void ButtonClickEventHandler(object sender, EventArgs e);

    class GuiButton : GuiComponentText
    {
        public event ButtonClickEventHandler ButtonClickEvent;

        public GuiButton(string textureName = "button2") : base(textureName)
        {
            this.width = 220;
            this.height = 60;
            this.color = Color.White;
        }

        public override void OnHold(MouseState mouseState)
        {
            this.color = Color.Gray;
        }

        public override void OnClick(MouseState mouseState)
        {
            SoundManager.PlaySound("select");
            ButtonClickEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnRelease()
        {
            this.color = Color.White;
        }

        public override void OnHover()
        {
            this.color = Color.LightGray;
        }

        public override void OnUnhover()
        {
            this.color = Color.White;
        }
    }
}
