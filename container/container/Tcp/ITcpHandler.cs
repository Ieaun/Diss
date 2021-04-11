namespace LoraWAN_Pipeline.Tcp
{
    using System;
    using SimpleTcp;
    public interface ITcpHandler
    {
        void Connected(object sender, EventArgs e);

        void DataReceived(object sender, DataReceivedEventArgs e);

        void Disconnected(object sender, EventArgs e);

        void Start();
    }
}
