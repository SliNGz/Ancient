using ancient.game.client.gui.component;
using ancient.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui
{
    public class Gui
    {
        protected GuiManager guiManager;
        protected List<GuiComponent> components;
        protected Queue<GuiComponent> componentsToAdd;
        protected Queue<GuiComponent> componentsToRemove;

        protected bool isCursorVisible;

        protected Gui lastGui;
        protected bool drawWorldBehind;
        protected Color backgroundColor;

        public Gui(GuiManager guiManager, string name)
        {
            this.guiManager = guiManager;
            this.components = new List<GuiComponent>();
            this.componentsToAdd = new Queue<GuiComponent>();
            this.componentsToRemove = new Queue<GuiComponent>();
            this.isCursorVisible = true;
            this.lastGui = null;
            this.drawWorldBehind = true;

            if (name != null)
                GuiManager.guis.Add(name, this);
        }

        public virtual void Initialize()
        {
            this.components.Clear();
        }

        public virtual void Update(MouseState mouseState)
        {
            while (componentsToAdd.Count > 0)
                components.Add(componentsToAdd.Dequeue());

            while (componentsToRemove.Count > 0)
                components.Remove(componentsToRemove.Dequeue());

            foreach (GuiComponent component in components)
                component.Update(mouseState);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GuiComponent component in components)
                component.Draw(spriteBatch);
        }

        public void AddComponent(GuiComponent component)
        {
            this.componentsToAdd.Enqueue(component);
        }

        public void RemoveComponent(GuiComponent component)
        {
            this.componentsToRemove.Enqueue(component);
        }

        public bool IsCursorVisible()
        {
            return this.isCursorVisible;
        }

        public void SetLastGui(Gui lastGui)
        {
            this.lastGui = lastGui;
        }

        public Gui GetLastGui()
        {
            return this.lastGui;
        }

        public bool ShouldDrawWorldBehind()
        {
            return this.drawWorldBehind;
        }

        public virtual bool CanClose()
        {
            return true;
        }

        public Color GetBackgroundColor()
        {
            return this.backgroundColor;
        }

        public virtual void OnDisplay(Gui lastGui)
        { }

        public virtual void OnClose()
        { }

        public virtual void Draw3D()
        { }

        public virtual bool Draw3DFromGuiManager()
        {
            return true;
        }
    }
}
