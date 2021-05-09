namespace LoRaSimulator
{
    using LoRaSimulator.Tcp;
    using SimpleUdp;

    public interface IUdpHandler
    {
        void DatagramReceived(object sender, Datagram dg);

        void Send();

        void Start();
    }
}
