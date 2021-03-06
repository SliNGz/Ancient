using ancient.game.client.gui.component;
using ancient.game.client.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.renderer.font;
using ancient.game.input;
using ancientlib.game.command;
using ancient.game.client.input.utils;
using ancient.game.client.network;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.network.packet.common.chat;
using ancientlib.game.utils.chat;

namespace ancient.game.client.gui
{
    public class GuiChatInput : Gui, IKeyboardSubscriber
    {
        private static int LETTERS_COUNT = 28;
        private Texture2D chatTexture;
        private GuiTexture chatGuiTexture;

        private GuiText text;

        public GuiChatInput(GuiManager guiManager) : base(guiManager, "chat_input")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;

            this.chatTexture = new Texture2D(Ancient.ancient.GraphicsDevice, FontRenderer.CHAR_SIZE * GuiChat.FONT_SIZE * LETTERS_COUNT, FontRenderer.CHAR_SIZE * GuiChat.FONT_SIZE + 2);

            Color[] data = new Color[chatTexture.Width * chatTexture.Height];

            for (int i = 0; i < chatTexture.Width; i++)
            {
                for (int j = 0; j < chatTexture.Height; j++)
                {
                    data[i + j * chatTexture.Width] = new Color(31, 31, 31, 225);
                }
            }

            this.chatTexture.SetData(data);

            this.chatGuiTexture = new GuiTexture(chatTexture);
            this.chatGuiTexture.SetY(1 - GuiUtils.GetRelativeYFromY(chatGuiTexture.GetHeight()));
            this.components.Add(chatGuiTexture);

            this.text = new GuiText();
            this.text.SetY(1 - GuiUtils.GetRelativeYFromY(text.GetHeight()));
            this.components.Add(text);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            guiManager.chat.SetChatVisible();
        }

        public override void OnDisplay(Gui lastGui)
        {
            base.OnDisplay(lastGui);
            Ancient.ancient.keyDispatcher.Subscriber = this;
            guiManager.chat.linesToRender = 10;
        }

        public override void OnClose()
        {
            base.OnClose();
            Ancient.ancient.keyDispatcher.Subscriber = null;
            guiManager.chat.linesToRender = 5;
        }

        public override bool CanClose()
        {
            return text.GetText() == "";
        }

        public void RecieveTextInput(char inputChar)
        {
            if (this.text.GetText().Length < LETTERS_COUNT)
                this.text.Add(inputChar);
        }

        public void RecieveTextInput(string text)
        { }

        public void RecieveCommandInput(char command)
        { }

        public void RecieveSpecialInput(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    text.Delete();
                    break;
                case Keys.Enter:
                    OnEnterPressed();
                    break;
                case Keys.Tab:
                    OnTabPressed();
                    break;
                case Keys.Escape:
                    OnEscapePressed();
                    break;
            }
        }

        private void OnEnterPressed()
        {
            if (text.GetText().Length == 0)
                return;

            ChatComponentText textComponent = new ChatComponentText(text.GetText(), Color.White);

            if (!Ancient.ancient.world.IsRemote())
            {
                if (text.GetText().StartsWith("/"))
                    CommandHandler.HandleText(Ancient.ancient.player, text.GetText());
                else
                {
                    textComponent.SetText(Ancient.ancient.player.GetName() + ": " + textComponent.GetText());
                    Ancient.ancient.world.AddChatComponent(textComponent);
                }
            }
            else
                NetClient.instance.SendPacket(new PacketChatComponent(textComponent));

            text.SetText("");
        }

        private void OnTabPressed()
        {
            if (this.text.GetText().Length == 0)
                return;

            string[] args = this.text.GetText().Split(' ');
            string text = args[args.Length - 1];

            if (text.StartsWith("/"))
            {
                text = text.Remove(0, 1);

                try
                {
                    text = CommandHandler.commands.First(x => x.Key.StartsWith(text)).Key;
                    text = "/" + text;
                    this.text.SetText(text);
                }
                catch (InvalidOperationException)
                { }
            }
        }

        private void OnEscapePressed()
        {
            this.text.SetText("");
        }
    }
}
