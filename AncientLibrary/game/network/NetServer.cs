using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.network
{
    public class NetServer
    {
        public static NetServer instance;
        public static int readThreadNum = 0;

        private TcpListener tcpListener;

        private List<NetConnection> netConnections;

        public NetServer(int port)
        {
            instance = this;

            this.netConnections = new List<NetConnection>();

            this.tcpListener = new TcpListener(GetLocalIPAddress(), port);
            this.tcpListener.Start();

            ThreadUtils.CreateThread("Listen Thread", ListenForClients, true).Start();

            Console.WriteLine("Starting server...");
            Console.WriteLine("Listening on IP: " + GetLocalIPAddress() + ", Port: " + port);
        }

        public void Update()
        {
            for (int i = 0; i < netConnections.Count; i++)
                netConnections[i].Update();
        }

        private void ListenForClients()
        {
            while (true)
                NetValidate.ValidateClient(new NetConnection(tcpListener.AcceptTcpClient()));
        }

        public void AddNetConnection(NetConnection netConnection)
        {
            this.netConnections.Add(netConnection);
        }

        public void RemoveNetConnection(NetConnection netConnection)
        {
            this.netConnections.Remove(netConnection);
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }

            throw new Exception("Local IP Address Not Found!");
        }
    }
}
