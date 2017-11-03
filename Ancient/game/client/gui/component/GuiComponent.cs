using ancient.game.client.renderer.font;
using ancient.game.client.utils;
using ancient.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.gui.component
{
    public abstract class GuiComponent
    {
        public static GuiComponent focusedComponent;

        protected float x;
        protected float y;

        protected int width;
        protected int height;

        protected Texture2D texture;
        protected Color color;

        protected bool didHover;
        protected bool held; // Enables grabbing of the component

        public GuiComponent()
        {
            this.color = Color.White;
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void OnClick(MouseState mouseState);

        public abstract void OnFocus();

        public abstract void OnHold(MouseState mouseState);

        public abstract void OnHover();

        public abstract void OnRelease();

        public abstract void OnUnfocus();

        public abstract void OnUnhover();

        public virtual void Update(MouseState mouseState)
        {
            if (InBounds(mouseState.Position.ToVector2()))
            {
                if (InputHandler.IsLeftButtonHeld())
                    held = true;
                else if (InputHandler.IsLeftButtonPressed())
                {
                    if (this != focusedComponent)
                    {
                        OnFocus();
                        focusedComponent = this;
                    }

                    OnClick(mouseState);
                }
                else if (InputHandler.IsLeftButtonReleased())
                    OnRelease();
                else
                {
                    OnHover();
                    didHover = true;
                }
            }
            else
            {
                if (InputHandler.IsLeftButtonPressed())
                {
                    if (this == focusedComponent)
                    {
                        OnUnfocus();
                        focusedComponent = null;
                    }
                }

                if (didHover)
                {
                    OnUnhover();
                    didHover = false;
                }
            }

            if (held)
            {
                OnHold(mouseState);

                if (!InputHandler.IsLeftButtonHeld())
                    held = false;
            }
        }

        public float GetX()
        {
            return this.x;
        }

        public GuiComponent SetX(float x)
        {
            this.x = x;
            return this;
        }

        public GuiComponent AddX(float add)
        {
            this.x += add;
            return this;
        }

        public float GetY()
        {
            return this.y;
        }

        public GuiComponent SetY(float y)
        {
            this.y = y;
            return this;
        }

        public GuiComponent AddY(float add)
        {
            this.y += add;
            return this;
        }

        public virtual int GetWidth()
        {
            return this.width;
        }

        public GuiComponent SetWidth(int width)
        {
            this.width = width;
            return this;
        }

        public virtual int GetHeight()
        {
            return this.height;
        }

        public GuiComponent SetHeight(int height)
        {
            this.height = height;
            return this;
        }

        public Texture2D GetTexture()
        {
            return this.texture;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public bool InBounds(Vector2 position)
        {
            int x = GuiUtils.GetXFromRelativeX(this.x);
            int y = GuiUtils.GetYFromRelativeY(this.y);
            return position.X >= x && position.Y >= y && position.X <= x + this.width && position.Y <= y + this.height;
        }

        public virtual GuiComponent Centralize()
        {
            this.x = 0.5F - GuiUtils.GetRelativeXFromX(GetWidth() / 2F);
            this.y = 0.5F - GuiUtils.GetRelativeYFromY(GetHeight() / 2F);
            return this;
        }

        public void ScaleToMatchResolution()
        {
            this.width = ((int)(width * Math.Round(Ancient.ancient.GraphicsDevice.Viewport.Width / (float)GuiUtils.DefaultWidth)));
            this.height = ((int)(height * Math.Round(Ancient.ancient.GraphicsDevice.Viewport.Height / (float)GuiUtils.DefaultHeight)));
        }

        public Color GetColor()
        {
            return this.color;
        }

        public GuiComponent SetColor(Color color)
        {
            this.color = color;
            return this;
        }
    }
}
