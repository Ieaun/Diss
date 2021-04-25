namespace DownlinkService.Queue
{
    using System;
    using EasyNetQ;
    using MediatR;
    using Serilog;
    using DownlinkService.Notifications;

    public class QueueAgent : IQueue
    {
        private IBus _bus;
        private ILogger _logger;
        private IMediator _mediator;

        public QueueAgent(IBus bus, ILogger logger, IMediator mediator)
        {
            this._mediator = mediator;
            this._logger = logger;
            this._bus = bus;

            //this WILL throw an error if rabbit mq is not running. Rabbit mq must be running.
            this._bus.PubSub.Subscribe<QueueTypes.Queues.Downlink>("Downlink", OnHandleNotification, x=> x.WithTopic(nameof(QueueTypes.Queues.Downlink)));
            this._bus.Advanced.Connected += OnConnected;
        }

        private void OnConnected(object sender, ConnectedEventArgs e)
        {
            _bus.PubSub.Subscribe<QueueTypes.Queues.Downlink>("Downlink", OnHandleNotification, x => x.WithTopic(nameof(QueueTypes.Queues.Downlink)));
        }

        public void OnHandleNotification(QueueTypes.Queues.Downlink packet)
        {
            _logger.Information("Received packet {@Packet}", packet);
            try
            {
                _mediator.Publish(new Notification
                {
                    Id = 1,
                    Payload = packet.Packet
                });
            }
            catch (Exception e)
            {
                _logger.Error("Failed to handle packet {@Packet} with exception {@Exception}", packet, e);
            }
            _logger.Information("Finished handling packet {@Packet}", packet);
        }
    }
}
