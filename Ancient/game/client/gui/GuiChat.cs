using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.client.renderer.font;
using ancient.game.client.utils;
using Microsoft.Xna.Framework;
using ancient.game.client.gui.component;
using Microsoft.Xna.Framework.Input;
using ancientlib.game.utils.chat;
using ancient.game.client.renderer.chat;
using ancientlib.game.init;

namespace ancient.game.client.gui
{
    public class GuiChat : Gui
    {
        public static int FONT_SIZE = 3;
        private List<ChatComponent> chat;

        public int linesToRender;

        public GuiChat(GuiManager guiManager) : base(guiManager, "chat")
        {
            this.chat = new List<ChatComponent>();
            this.chat.Add(new ChatComponentItem(Items.staff));
            linesToRender = 5;
        }

        public override void Initialize()
        {
            ChatRenderItem.Initialize();
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            for (int i = 0; i < chat.Count; i++)
                chat[i].Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            int y = GuiUtils.GetYFromRelativeY(guiManager.ingame.itemInHand.GetY()) - FontRenderer.CHAR_SIZE * FONT_SIZE - 5;

            for (int i = Math.Max(0, chat.Count - linesToRender); i < chat.Count; i++)
            {
                ChatComponent chatComponent = chat[i];
                int x = 3;

                if (chatComponent is ChatComponentItem)
                    x = 20;

                ChatRenderer.Draw(spriteBatch, chat[i], x, y - (chat.Count - 1 - i) * FontRenderer.CHAR_SIZE * FONT_SIZE);
            }
        }

        public override void Draw3D()
        {
            base.Draw3D();
            for (int i = 0; i < chat.Count; i++)
                ChatRenderer.Draw3D(chat[i]);
        }

        public void AddComponent(ChatComponent chatComponent)
        {
            if (chat.Count > 10)
                chat.RemoveAt(0);

            chat.Add(chatComponent);
        }

        public void SetChatVisible()
        {
            for (int i = 0; i < chat.Count; i++)
                chat[i].alpha = ChatComponent.MAX_ALPHA;
        }

        public void Clear()
        {
            this.chat.Clear();
            linesToRender = 5;
        }
    }
}
