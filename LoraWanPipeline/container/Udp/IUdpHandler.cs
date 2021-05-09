namespace LoraWAN_Pipeline.Udp
{
    using System;
    using SimpleTcp;
    using SimpleUdp;

    public interface IUdpHandler
    {
        void DatagramReceived(object sender, Datagram dg);

        void EndpointDetected(object sender, EndpointMetadata md);

        void Start();
    }
}
