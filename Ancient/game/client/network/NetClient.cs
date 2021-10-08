using ancientlib.game.network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.handshake;
using ancient.game.client.gui;
using ancient.game.client.utils;

namespace ancient.game.client.network
{
    public class NetClient
    {
        public static NetClient instance;

        private NetConnection netConnection;

        public NetClient()
        {
            instance = this;
        }

        public void Update()
        {
            if (IsConnected())
                netConnection.Update();
        }

        public bool TryConnect(string ip, int port)
        {
            try
            {
                this.netConnection = new NetConnection(new TcpClient(ip, port));
                this.netConnection.IOExceptionCaughtEvent += new IOExceptionCaughtEventHandler(OnCatchIOException);
                this.netConnection.player = Ancient.ancient.player;
                //this.netConnection.SetUser(Ancient.ancient.user);
                InitializeConnection();
                return true;
            }
            catch (Exception e)
            { }

            return false;
        }

        private void InitializeConnection()
        {
            Ancient.ancient.inputManager.playerInput = Ancient.ancient.inputManager.onlineInput;

            SendPacket(new PacketHandshake(netConnection));
            netConnection.StartReadingPackets();
        }

        public void SendPacket(Packet packet)
        {
            netConnection.SendPacket(packet);
        }

        public bool IsConnected()
        {
            return netConnection == null ? false : netConnection.IsConnected();
        }

        public NetConnection GetNetConnection()
        {
            return this.netConnection;
        }

        public void Close()
        {
            if (netConnection != null)
                netConnection.Close();
        }

        public void OnCatchIOException(object sender, EventArgs e)
        {
            Ancient.ancient.guiManager.DisplayGui(new GuiDisconnect(Ancient.ancient.guiManager, "Lost connection."));
        }
    }
}
