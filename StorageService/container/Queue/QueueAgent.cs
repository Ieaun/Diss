namespace StorageService.Queue
{
    using System;
    using EasyNetQ;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using StorageService.Notifications;
    using StorageService.Notifications.ReceivedPackets;
    using StorageService.ReceivedPacketNotifications;
    using StorageService.ReceivedPacketNotifications.LoRaWanPacketSections;

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
            this._bus.PubSub.Subscribe<QueueTypes.Queues.Storages.Storage>("Storage", OnHandleNotification, x=> x.WithTopic(nameof(QueueTypes.Queues.Storages.Storage)));
            this._bus.Advanced.Connected += OnConnected;
        }

        private void OnConnected(object sender, ConnectedEventArgs e)
        {
            _bus.PubSub.Subscribe<QueueTypes.Queues.Storages.Storage>("Storage", OnHandleNotification, x => x.WithTopic(nameof(QueueTypes.Queues.Storages.Storage)));
        }

        public void OnHandleNotification(QueueTypes.Queues.Storages.Storage packet)
        {
            _logger.LogInformation("Received packet {@Packet}", packet);
            var newPacket = MapFromUplink(packet);
            try
            {
                _mediator.Publish(new NewPacketNotification { Packet = newPacket });
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to handle packet {@Packet} with exception {@Exception}", packet, e);
            }
            _logger.LogInformation("Finished handling packet {@Packet}", packet);
        }

        public NewPacket MapFromUplink(QueueTypes.Queues.Storages.Storage packet)
        {
            return new NewPacket
            {
                PacketType = "rxpk",
                Uplink = new ReceivedPacket
                {
                    metadata = new RecievedPacketMetadata
                    {
                        Channel = packet.Packet.Uplink.metadata.Channel,
                        CodingRate = packet.Packet.Uplink.metadata.CodingRate,
                        DataRate = packet.Packet.Uplink.metadata.DataRate,
                        Frequency = packet.Packet.Uplink.metadata.Frequency,
                        LuminanceSignalToRatio = packet.Packet.Uplink.metadata.LuminanceSignalToRatio,
                        Modulation = packet.Packet.Uplink.metadata.Modulation,
                        RadioFrequencyChannel = packet.Packet.Uplink.metadata.RadioFrequencyChannel,
                        RecievedSignalStrenghtIndicator = packet.Packet.Uplink.metadata.RecievedSignalStrenghtIndicator,
                        Size = packet.Packet.Uplink.metadata.Size,
                        Stat = packet.Packet.Uplink.metadata.Stat,
                        Timestamp = packet.Packet.Uplink.metadata.Timestamp,
                        Data = packet.Packet.Uplink.metadata.Data
                    },
                    isRegistered = packet.Packet.Uplink.isRegistered,
                    decodedPacket = new DecodedLoraPacket
                    {
                        PhysicalPayload = new PhysicalPayload
                        {
                            MacHeader = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacHeader,
                            MIC = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MIC,
                            MacPayload = new MacPayload
                            {
                                FramePort = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacPayload.FramePort,
                                FramePayload = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacPayload.FramePayload,
                                FrameHeader = new FrameHeader
                                {
                                    DeviceAddress = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.DeviceAddress,
                                    FrameControlOctet = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameControlOctet,
                                    FrameCounter = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameCounter,
                                    FrameOptions = packet.Packet.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameOptions
                                }
                            }
                        },
                        OriginalHexString = packet.Packet.Uplink.decodedPacket.OriginalHexString
                    },
                    OriginalMessage = packet.Packet.Uplink.OriginalMessage
                }
            };
        }
    }
}
