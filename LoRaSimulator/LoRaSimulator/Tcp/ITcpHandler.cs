namespace LoRaSimulator
{
    using SimpleTcp;
    using LoRaSimulator.Tcp;
    public interface ITcpHandler
    {
        void DataReceived(object sender, DataReceivedEventArgs e);

        void ClientConnected(object sender, ClientConnectedEventArgs e);

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e);

        void Send(LoraPacket packet);

        void Start();
    }
}
