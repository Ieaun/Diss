namespace UplinkService.Queue
{
    using System;
    using EasyNetQ;
    using MediatR;
    using Serilog;
    using UplinkService.Notifications;

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
            this._bus.PubSub.Subscribe<QueueTypes.Queues.Uplinks.Uplink>("Uplink", OnHandleNotification, x=> x.WithTopic(nameof(QueueTypes.Queues.Uplinks.Uplink)));
            this._bus.Advanced.Connected += OnConnected;
        }

        private void OnConnected(object sender, ConnectedEventArgs e)
        {
            _bus.PubSub.Subscribe<QueueTypes.Queues.Uplinks.Uplink>("Uplink", OnHandleNotification, x => x.WithTopic(nameof(QueueTypes.Queues.Uplinks.Uplink)));
        }

        public void OnHandleNotification(QueueTypes.Queues.Uplinks.Uplink packet)
        {
            _logger.Information("Received packet {@Packet}", packet);
            try
            {
                if (packet.PacketType == "rxpk") {
                    _mediator.Publish(new Notification
                    {
                        PacketType = packet.PacketType,
                        isRegisteredDevice = packet.isRegistered,
                        RxMetadata = new ReceivedPacketMetadata
                        {
                            Channel = packet.RxPacket.metadata.Channel,
                            CodingRate = packet.RxPacket.metadata.CodingRate,
                            Data = packet.RxPacket.metadata.Data,
                            DataRate = packet.RxPacket.metadata.DataRate,
                            Frequency = packet.RxPacket.metadata.Frequency,
                            LuminanceSignalToRatio = packet.RxPacket.metadata.LuminanceSignalToRatio,
                            Modulation = packet.RxPacket.metadata.Modulation,
                            Size = packet.RxPacket.metadata.Size,
                            Timestamp = packet.RxPacket.metadata.Timestamp,
                            RecievedSignalStrenghtIndicator = packet.RxPacket.metadata.RecievedSignalStrenghtIndicator,
                            RadioFrequencyChannel = packet.RxPacket.metadata.RadioFrequencyChannel,
                            Stat = packet.RxPacket.metadata.Stat
                        },
                        OriginalMessage = packet.RxPacket.OriginalMessage,
                        UnalteredPacket = packet.UnalteredPacket
                    });
                }

                if (packet.PacketType == "stat")
                {
                    _mediator.Publish(new Notification
                    {
                        PacketType = packet.PacketType,
                        GsMetadata = packet.GsPacket.metadata!= null? new GatewayStatusUpdateMetadata { 
                            Ackr = packet.GsPacket.metadata.Ackr,
                            Altitude = packet.GsPacket.metadata.Altitude,
                            Description = packet.GsPacket.metadata.Description,
                            Dwnb = packet.GsPacket.metadata.Dwnb,
                            Email = packet.GsPacket.metadata.Email,
                            Latitude = packet.GsPacket.metadata.Latitude,
                            Longitude = packet.GsPacket.metadata.Longitude,
                            pfrm = packet.GsPacket.metadata.pfrm,
                            Rxfw = packet.GsPacket.metadata.Rxfw,
                            Rxnb = packet.GsPacket.metadata.Rxnb,
                            Rxok = packet.GsPacket.metadata.Rxok,
                            Time = packet.GsPacket.metadata.Time,
                            Txnb = packet.GsPacket.metadata.Txnb
                        }: null,
                        OriginalMessage = packet.GsPacket.OriginalMessage,
                        UnalteredPacket = packet.UnalteredPacket
                    });
                }

            }
            catch (Exception e)
            {
                _logger.Error("Failed to handle packet {@Packet} with exception {@Exception}", packet, e);
            }
            _logger.Information("Finished handling packet {@Packet}", packet);
        }
    }
}
