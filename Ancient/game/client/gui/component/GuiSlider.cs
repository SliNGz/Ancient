using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.renderer.texture;
using Microsoft.Xna.Framework;
using ancient.game.client.utils;

namespace ancient.game.client.gui.component
{
    public delegate void ValueChangedEventHandler(object sender, EventArgs e);

    class GuiSlider : GuiComponentText
    {
        public event ValueChangedEventHandler ValueChanged;

        private float value;

        public GuiSlider() : base("button2")
        {
            this.value = 1;
            this.width = 220;
            this.height = 60;
        }

        public override void OnClick(MouseState mouseState)
        {
            base.OnClick(mouseState);

            int x = mouseState.X - GuiUtils.GetXFromRelativeX(this.x);
            SetValue((float)Math.Round(((x / (float)this.width) * 100)) / 100F);
        }

        public override void OnHold(MouseState mouseState)
        {
            base.OnHold(mouseState);

            int x = mouseState.X - GuiUtils.GetXFromRelativeX(this.x);
            SetValue((float)Math.Round(((x / (float)this.width) * 100)) / 100F);
        }

        public float GetValue()
        {
            return this.value;
        }

        public void SetValue(float value)
        {
            this.value = MathHelper.Clamp(value, 0, 1);

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
