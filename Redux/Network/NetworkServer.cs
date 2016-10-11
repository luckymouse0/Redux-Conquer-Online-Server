using System;
using System.Net;
using System.Net.Sockets;


namespace Redux.Network
{
    // Magic. Do not touch.
    public class NetworkServer
    {
        public Socket Socket { get; private set; }
        public IPEndPoint LocalEndPoint { get; private set; }
        public string Name { get; private set; }

        public NetworkClientConnection OnConnect;
        public NetworkClientReceive OnReceive;
        public NetworkClientConnection OnDisconnect;
        public BruteforceProtection AttackProtector;

        public int ClientBufferSize;

        public NetworkServer(string name)
        {
            Name = name;
            AttackProtector = new BruteforceProtection(Constants.MAX_CONNECTIONS_PER_MINUTE, Constants.MINUTES_BANNED_BRUTEFORCE);
        }

        public void Prepare(int port, int backlog)
        {
            LocalEndPoint = new IPEndPoint(IPAddress.Any, port);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(LocalEndPoint);
            Socket.Listen(backlog);
        }

        public void BeginAccept()
        {
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, false);
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            Socket.BeginAccept(Accept, null);
        }

        private void Accept(IAsyncResult result)
        {
            Socket clientSocket;
            try
            {
                clientSocket = Socket.EndAccept(result);
            }
            catch (SocketException)
            {
                BeginAccept();
                return;
            }

            if (AttackProtector.Authenticate(clientSocket))
            {
                clientSocket.ReceiveBufferSize = ClientBufferSize;
                var client = new NetworkClient(this, clientSocket, ClientBufferSize);
                InvokeOnConnect(client);
                client.BeginReceive();
            }
            else clientSocket.Disconnect(false);
            BeginAccept();
        }

        public void InvokeOnConnect(NetworkClient client)
        {
            if (OnConnect != null) OnConnect(client);
        }

        public void InvokeOnReceive(NetworkClient client, byte[] packet)
        {
            if (OnReceive != null) OnReceive(client, packet);
        }

        public void InvokeOnDisconnect(NetworkClient client)
        {
            if (!client.Alive) return;

            client.Alive = false;

            if (OnDisconnect != null) OnDisconnect(client);
        }
    }
}
