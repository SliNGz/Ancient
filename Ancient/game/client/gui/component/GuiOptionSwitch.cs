using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ancient.game.client.utils;

namespace ancient.game.client.gui.component
{
    public delegate void OptionChangedEventHandler(object sender, EventArgs e);

    class GuiOptionSwitch : GuiComponentText
    {
        private int optionIndex;
        private List<object> options;

        public event OptionChangedEventHandler OptionChanged;

        public GuiOptionSwitch() : base("button2")
        {
            this.optionIndex = 0;
            this.options = new List<object>();
        }

        public override void OnClick(MouseState mouseState)
        {
            base.OnClick(mouseState);

            Vector2 mousePosition = mouseState.Position.ToVector2();
            mousePosition -= new Vector2(GuiUtils.GetXFromRelativeX(x), GuiUtils.GetYFromRelativeY(y));

            if (mousePosition.X < width / 2F)
                AddOptionIndex(-1);
            else
                AddOptionIndex(1);
        }

        public void AddOption(object obj)
        {
            this.options.Add(obj);
        }

        public object GetSelectedOption()
        {
            if (options.Count == 0)
                return null;

            return this.options[optionIndex];
        }

        public void SetOptionIndex(int optionIndex)
        {
            if (options.Count == 0)
                return;

            this.optionIndex = (optionIndex % options.Count + options.Count) % options.Count;

            OptionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddOptionIndex(int add)
        {
            SetOptionIndex(optionIndex + add);
        }
    }
}
