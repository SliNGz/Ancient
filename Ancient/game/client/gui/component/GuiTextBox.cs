using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.input.utils;
using ancient.game.client.utils;
using Microsoft.Xna.Framework.Graphics;

namespace ancient.game.client.gui.component
{
    public delegate void TextChangedEventHandler(object sender, EventArgs e);

    public class GuiTextBox : GuiComponentText, IKeyboardSubscriber
    {
        public event TextChangedEventHandler TextChangedEvent;

        private GuiText textBox;

        private TextBoxInput input;

        public GuiTextBox() : base("button2")
        {
            this.textBox = new GuiText();
            this.input = TextBoxInput.ALL;
        }

        public GuiTextBox(TextBoxInput input) : base("button2")
        {
            this.textBox = new GuiText();
            this.input = input;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            textBox.Draw(spriteBatch);
        }

        public override void OnFocus()
        {
            base.OnFocus();
            Ancient.ancient.keyDispatcher.Subscriber = this;
        }

        public override void OnUnfocus()
        {
            base.OnUnfocus();
            Ancient.ancient.keyDispatcher.Subscriber = null;
        }

        public void RecieveCommandInput(char command)
        { }

        public void RecieveSpecialInput(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    textBox.Delete();
                    OnTextChanged();
                    break;
            }
        }

        public void RecieveTextInput(string text)
        { }

        public void RecieveTextInput(char inputChar)
        {
            switch (input)
            {
                case TextBoxInput.ALL:
                    this.textBox.Add(inputChar);
                    break;
                case TextBoxInput.NUMBER:
                    this.textBox.AddNumber(inputChar);
                    break;
            }

            OnTextChanged();
        }

        public void OnTextChanged()
        {
            TextChangedEvent?.Invoke(this, EventArgs.Empty);
            textBox.SetX(this.x + GuiUtils.GetRelativeXFromX(this.width / 2) - GuiUtils.GetRelativeXFromX(textBox.GetWidth() / 2));
            textBox.SetY(this.y + GuiUtils.GetRelativeYFromY(this.height / 2 - textBox.GetHeight() / 2));
        }

        public GuiText GetTextBox()
        {
            return this.textBox;
        }

        public void SetTextBox(GuiText textBox)
        {
            this.textBox = textBox;
        }
    }

    public enum TextBoxInput
    {
        ALL,
        NUMBER
    }
}
