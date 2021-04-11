namespace LoraWAN_Pipeline.Queue
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ;
    using MediatR;
    using Serilog;
    using LoraWAN_Pipeline.Notifications;
    using LoraWAN_Pipeline.Tcp;
    public class QueueAgent : IQueue
    {
        private IBus _bus;
        private ILogger _logger;
        private IMediator _mediator;
        private ITcpHandler _tcpHandler;

        public QueueAgent(IBus bus, ILogger logger, IMediator mediator)
        {
            this._mediator = mediator;
            this._logger = logger;
            this._bus = bus;


            //this WILL throw an error if rabbit mq is not running. Rabbit mq must be running. 
            this._bus.PubSub.Subscribe<QueueTypes.Queues.Downlink>("Downlink", OnNotificationReceived, x=> x.WithTopic(nameof(QueueTypes.Queues.Downlink)));
            this._bus.Advanced.Connected += OnConnected;
        }

        private void OnConnected(object sender, ConnectedEventArgs e)
        {
            _bus.PubSub.Subscribe<QueueTypes.Queues.Downlink>("Downlink", OnNotificationReceived, x => x.WithTopic(nameof(QueueTypes.Queues.Downlink)));
        }

        private void OnNotificationReceived(QueueTypes.Queues.Downlink packet)
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

        public async Task EnqueueToDownlink(Notification notification)
        {
            _logger.Information("Publishing notification {@Notification}", notification);
            await _bus.PubSub.PublishAsync(notification, handler => handler.WithTopic(nameof(Notification)));
        }

        public async Task EnqueueToUplink(LoraPacket receivedPacket)
        {
            var packet = MapToQueueType(receivedPacket);

            _logger.Information("Publishing notification {@Notification}", packet);
            await _bus.PubSub.PublishAsync(packet, handler => handler.WithTopic(nameof(QueueTypes.Queues.Downlink)));
        }

        public async Task EnqueueToStorage(LoraPacket receivedPacket)
        {
            var packet = MapToQueueType(receivedPacket);

            _logger.Information("Publishing notification {@Notification}", packet);
            await _bus.PubSub.PublishAsync(packet, handler => handler.WithTopic(nameof(QueueTypes.Models.LoraPacket)));
        }

        public QueueTypes.Queues.Downlink MapToQueueType(LoraPacket receivedPacket)
        {
            return new QueueTypes.Queues.Downlink
            {
                Packet = new QueueTypes.Models.LoraPacket
                {
                    Payload = receivedPacket.Payload
                }
            };
        }
    }
}
