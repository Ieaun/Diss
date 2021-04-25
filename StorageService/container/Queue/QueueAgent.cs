namespace StorageService.Queue
{
    using System;
    using EasyNetQ;
    using MediatR;
    using Microsoft.Extensions.Logging;

    using StorageService.Notifications;
    public class QueueAgent : IQueue
    {
        private IBus _bus;
        private ILogger<QueueAgent> _logger;
        private IMediator _mediator;

        public QueueAgent(IBus bus, ILogger<QueueAgent> logger, IMediator mediator)
        {
            this._mediator = mediator;
            this._logger = logger;
            this._bus = bus;

            //this WILL throw an error if rabbit mq is not running. Rabbit mq must be running.
            this._bus.PubSub.Subscribe<QueueTypes.Queues.Storage>("Storage", OnHandleNotification, x=> x.WithTopic(nameof(QueueTypes.Queues.Storage)));
            this._bus.Advanced.Connected += OnConnected;
        }

        private void OnConnected(object sender, ConnectedEventArgs e)
        {
            _bus.PubSub.Subscribe<QueueTypes.Queues.Storage>("Storage", OnHandleNotification, x => x.WithTopic(nameof(QueueTypes.Queues.Storage)));
        }

        public void OnHandleNotification(QueueTypes.Queues.Storage packet)
        {
            _logger.LogInformation("Received packet {@Packet}", packet);
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
                _logger.LogError("Failed to handle packet {@Packet} with exception {@Exception}", packet, e);
            }
            _logger.LogInformation("Finished handling packet {@Packet}", packet);
        }
    }
}
