namespace Redux.Network
{
    public delegate void NetworkClientConnection(NetworkClient client);
    public delegate void NetworkClientReceive(NetworkClient client, byte[] buffer);
}
