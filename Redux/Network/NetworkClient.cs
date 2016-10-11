using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Redux.Network
{
    public class NetworkClient
    {
        public Socket Socket { get; private set; }
        public NetworkServer Server { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        private readonly byte[] _buffer;
        public object Owner;
        public bool Alive;
        public NetworkClient(NetworkServer server, Socket socket, int bufferLength)
        {
            Alive = true;
            Server = server;
            Socket = socket;
            _buffer = new byte[bufferLength];
            RemoteEndPoint = (IPEndPoint)Socket.RemoteEndPoint;
        }
        public void BeginReceive()
        {
            try
            {
                Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);
            }
            catch (SocketException)
            {
                Server.InvokeOnDisconnect(this);
            }
        }

        private void Receive(IAsyncResult result)
        {
            if (Socket != null)
            {
                try
                {
                    SocketError error;
                    int length = Socket.EndReceive(result, out error);
                    if (Alive && error == SocketError.Success)
                    {
                        if (length > 0)
                        {
                            var received = new byte[length];
                            unsafe
                            {
                                fixed (byte* pReceive = received, pBuf = _buffer)
                                {
                                    MSVCRT.memcpy(pReceive, pBuf, length);
                                }
                            }
                            Server.InvokeOnReceive(this, received);

                            BeginReceive();
                        }
                        else
                        {
                            Server.InvokeOnDisconnect(this);
                        }
                    }
                }
                catch (SocketException)
                {
                    Server.InvokeOnDisconnect(this);
                }
            }
        }

        public void Send(byte[] packet)
        {
            if (Alive)
            {
                try
                {
                    Socket.BeginSend(packet, 0, packet.Length, SocketFlags.None, new AsyncCallback(EndSend), null);
                }
                catch (SocketException)
                {
                    Server.InvokeOnDisconnect(this);
                }
            }
        }

        private void EndSend(IAsyncResult result)
        {
            try
            {
                Socket.EndSend(result);
            }
            catch (SocketException)
            {
                Server.InvokeOnDisconnect(this);
            }
        }

        public void Disconnect()
        {
            try
            {
                Socket.Disconnect(false);
            }
            catch (SocketException)
            {
            }
            Server.InvokeOnDisconnect(this);
        }

        public override string ToString()
        {
            return RemoteEndPoint.ToString();
        }
    }
}
