using ancient.game.entity.player;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.handler;
using ancientlib.game.user;
using ancientlib.game.utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.network
{
    public delegate void IOExceptionCaughtEventHandler(object sender, EventArgs e);

    public class NetConnection
    {
        public event IOExceptionCaughtEventHandler IOExceptionCaughtEvent;

        public static bool PRINT_INCOMING_PACKETS = false;
        public static bool PRINT_OUTGOING_PACKETS = false;

        private ConcurrentQueue<Packet> outgoingPackets;

        private TcpClient tcpClient;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;

        public ConnectionStage stage;

        public EntityPlayer player;

        private User user;

        public NetConnection(TcpClient tcpClient)
        {
            this.outgoingPackets = new ConcurrentQueue<Packet>();

            this.tcpClient = tcpClient;
            this.tcpClient.NoDelay = true;
            this.stream = tcpClient.GetStream();
            this.writer = new BinaryWriter(stream);
            this.reader = new BinaryReader(stream);

            this.player = null;

            this.stage = ConnectionStage.HANDSHAKE;

            this.user = new User();
        }

        public void Update()
        {
            SendPackets();
        }

        public void SendPacket(Packet packet)
        {
            outgoingPackets.Enqueue(packet);
        }

        public void SendPackets()
        {
            try
            {
                while (outgoingPackets.Count > 0)
                {
                    Packet packet = null;
                    if (outgoingPackets.TryDequeue(out packet))
                    {
                        if (PRINT_OUTGOING_PACKETS)
                            ConsoleExtensions.WriteLine(ConsoleColor.Green, "SENDING: " + packet);

                        writer.Write(Packets.GetIDFromSendPacket(packet));
                        packet.Write(writer);
                    }
                }
            }
            catch { }
        }

        public void StartReadingPackets()
        {
            string name = "Read Thread";

            if (IsServer())
            {
                name += " " + NetServer.readThreadNum;
                NetServer.readThreadNum++;
            }

            ThreadUtils.CreateThread(name, ReadPackets, true).Start();
        }

        private void ReadPackets()
        {
            while (IsConnected())
            {
                try
                {
                    HandlePacket(ReadPacket());
                }
                catch (IOException)
                {
                    OnCatchIOException();
                }
            }
        }

        public Packet ReadPacket()
        {
            byte header = reader.ReadByte();

            Packet packet = Packets.GetRecievePacketFromID(header);

            if (PRINT_INCOMING_PACKETS)
                ConsoleExtensions.WriteLine(ConsoleColor.Cyan, "RECIEVED: " + packet);

            if (packet != null)
                packet.Read(reader);

            return packet;
        }

        private void HandlePacket(Packet packet)
        {
            PacketHandlers.GetPacketHandlerFromPacket(packet).HandlePacket(packet, this);
        }

        public void Close()
        {
            if (!IsConnected())
                return;

            this.tcpClient.Close();
        }

        public TcpClient GetTcpClient()
        {
            return this.tcpClient;
        }

        public NetworkStream GetNetworkStream()
        {
            return this.stream;
        }

        public BinaryWriter GetBinaryWriter()
        {
            return this.writer;
        }

        public BinaryReader GetBinaryReader()
        {
            return this.reader;
        }

        public bool IsConnected()
        {
            if (this.tcpClient != null)
                return this.tcpClient.Connected;

            return false;
        }

        public bool IsTimedout()
        {
            try
            {
                if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected)
                {
                    if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                            return false;
                        else
                            return true;
                    }

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsServer()
        {
            return NetServer.instance != null;
        }

        public override string ToString()
        {
            return tcpClient.Client.RemoteEndPoint.ToString();
        }

        public void OnCatchIOException()
        {
            IOExceptionCaughtEvent?.Invoke(this, EventArgs.Empty);
        }

        public EndPoint GetIPAddress()
        {
            return this.tcpClient.Client.RemoteEndPoint;
        }

        public User GetUser()
        {
            return this.user;
        }

        public void SetUser(User user)
        {
            this.user = user;
        }
    }

    public enum ConnectionStage
    {
        HANDSHAKE,
        CHARACTER_SELECTION,
        INGAME
    }
}
