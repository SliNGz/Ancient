using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.utils;
using Microsoft.Xna.Framework;

namespace ancient.game.client.gui.component
{
    public class GuiColorRGB : GuiComponent
    {
        public event TextChangedEventHandler TextChangedEvent;

        private GuiTextBox rBox;
        private GuiTextBox gBox;
        private GuiTextBox bBox;

        public GuiColorRGB(float x, float y)
        {
            this.rBox = new GuiTextBox(TextBoxInput.NUMBER);
            this.rBox.TextChangedEvent += new TextChangedEventHandler(OnTextChanged);
            this.rBox.SetX(x);
            this.rBox.SetY(y);
            this.rBox.SetWidth(70);
            this.rBox.SetHeight(50);
            this.rBox.GetTextBox().SetText("0");
            this.rBox.OnTextChanged();

            this.gBox = new GuiTextBox(TextBoxInput.NUMBER);
            this.gBox.TextChangedEvent += new TextChangedEventHandler(OnTextChanged);
            this.gBox.SetX(rBox.GetX() + GuiUtils.GetRelativeXFromX(rBox.GetWidth() + 10));
            this.gBox.SetY(y);
            this.gBox.SetWidth(70);
            this.gBox.SetHeight(50);
            this.gBox.GetTextBox().SetText("0");
            this.gBox.OnTextChanged();

            this.bBox = new GuiTextBox(TextBoxInput.NUMBER);
            this.bBox.TextChangedEvent += new TextChangedEventHandler(OnTextChanged);
            this.bBox.SetX(gBox.GetX() + GuiUtils.GetRelativeXFromX(gBox.GetWidth() + 10));
            this.bBox.SetY(y);
            this.bBox.SetWidth(70);
            this.bBox.SetHeight(50);
            this.bBox.GetTextBox().SetText("0");
            this.bBox.OnTextChanged();
        }

        public override void Update(MouseState mouseState)
        {
            rBox.Update(mouseState);
            gBox.Update(mouseState);
            bBox.Update(mouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            rBox.Draw(spriteBatch);
            gBox.Draw(spriteBatch);
            bBox.Draw(spriteBatch);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            GuiTextBox textBox = (GuiTextBox)sender;
            string text = textBox.GetTextBox().GetText();
            int value = 0;

            if (int.TryParse(text, out value))
            {
                value = MathHelper.Clamp(value, 0, 255);
                textBox.GetTextBox().SetText(value.ToString());

                TextChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color GetColorValue()
        {
            int r = 0;
            int.TryParse(rBox.GetTextBox().GetText(), out r);

            int g = 0;
            int.TryParse(gBox.GetTextBox().GetText(), out g);

            int b = 0;
            int.TryParse(bBox.GetTextBox().GetText(), out b);

            return new Color(r, g, b);
        }

        public override void OnClick(MouseState mouseState)
        { }

        public override void OnFocus()
        { }

        public override void OnHold(MouseState mouseState)
        { }

        public override void OnHover()
        { }

        public override void OnRelease()
        { }

        public override void OnUnfocus()
        { }

        public override void OnUnhover()
        { }
    }
}
