namespace LoraWAN_Pipeline.Queue
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ;
    using MediatR;
    using Serilog;
    using LoraWAN_Pipeline.Notifications;
    using LoraWAN_Pipeline.Tcp;
    using LoraWAN_Pipeline.Models;
    using LoraWAN_Pipeline.Notifications.ReceivedPackets;

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
                    datagram = null //packet.Packet
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

        public async Task EnqueueToUplink(ReceivedPacket receivedPacket)
        {
            //var packet = MapToUplinkType(receivedPacket);

            //_logger.Information("Publishing uplink notification {@Notification}", packet);
            //await _bus.PubSub.PublishAsync(packet, handler => handler.WithTopic(nameof(QueueTypes.Queues.Downlink)));
        }

        public async Task EnqueueToStorage(ISemtechUdpPacket receivedPacket)
        {
            //var packet = MapToStorageType(receivedPacket);

            //_logger.Information("Publishing storage notification {@Notification}", packet);
            //await _bus.PubSub.PublishAsync(packet, handler => handler.WithTopic(nameof(QueueTypes.Models.LoraPacket)));
        }

        public QueueTypes.Queues.Uplink MapToUplinkType(ReceivedPacket receivedPacket)
        {
            return new QueueTypes.Queues.Uplink
            {
                Packet = new QueueTypes.Models.LoraPacket
                {
                    //Payload = receivedPacket.Payload
                }
            };
        }

        public QueueTypes.Queues.Storage MapToStorageType(ISemtechUdpPacket receivedPacket)
        {
            return new QueueTypes.Queues.Storage
            {
                Packet = new QueueTypes.Models.LoraPacket
                {
                    //Payload = receivedPacket.Payload
                }
            };
        }
    }
}
