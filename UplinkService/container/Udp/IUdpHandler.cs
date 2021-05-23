namespace UplinkService.Udp
{
    using SimpleUdp;

    public interface IUdpHandler
    {
        void EndpointDetected(object sender, EndpointMetadata md);

        void Start();

        void Send(string address, int port, byte[] message);
    }
}
