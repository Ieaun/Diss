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
            this._bus.PubSub.Subscribe<QueueTypes.Queues.Downlinks.Downlink>("Downlink", OnNotificationReceived, x=> x.WithTopic(nameof(QueueTypes.Queues.Downlinks.Downlink)));
            this._bus.Advanced.Connected += OnConnected;
        }

        private void OnConnected(object sender, ConnectedEventArgs e)
        {
            _bus.PubSub.Subscribe<QueueTypes.Queues.Downlinks.Downlink>("Downlink", OnNotificationReceived, x => x.WithTopic(nameof(QueueTypes.Queues.Downlinks.Downlink)));
        }

        private void OnNotificationReceived(QueueTypes.Queues.Downlinks.Downlink packet)
        {
            _logger.Information("Received packet {@Packet}", packet);
            try
            {
                _mediator.Publish(new Notification
                {
                    // TODO: Handle downlink here
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

        public async Task EnqueueToUplink(NewPacket packet)
        {
            QueueTypes.Queues.Uplinks.Uplink uplinkPacket = null;

            if (packet.PacketType == "rxpk") {
                uplinkPacket = new QueueTypes.Queues.Uplinks.Uplink
                {
                    PacketType = "rxpk",
                    isRegistered = packet.RxPacket.isRegesteredDevice,
                    RxPacket = new QueueTypes.Models.ReceivedPackets.ReceivedPacket
                    {
                        metadata = new QueueTypes.Models.ReceivedPackets.ReceivedPacketsMetadata {
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
                        }               
                    }
                };
            }

            if (packet.PacketType == "stat")
            {
                uplinkPacket = new QueueTypes.Queues.Uplinks.Uplink
                {
                    PacketType = "stat",
                    GsPacket = new QueueTypes.Models.GatewayStatusUpdates.GatewayStatusUpdate
                    {
                        OriginalMessage = packet.StatPacket.OriginalMessage
                    }
                };
            }

            _logger.Information("Publishing uplink notification {@Notification}", uplinkPacket);

            await _bus.PubSub.PublishAsync(uplinkPacket, handler => handler.WithTopic(nameof(QueueTypes.Queues.Uplinks.Uplink)));
        }

        public async Task EnqueueToStorage(NewPacket packet) 
        {
            var storagePacket = new QueueTypes.Queues.Storages.Storage { 
                Packet = MapUplinkToQueueType(packet)
            };

            _logger.Information("Publishing storage notification {@Notification}", storagePacket);
            await _bus.PubSub.PublishAsync(storagePacket, handler => handler.WithTopic(nameof(QueueTypes.Queues.Storages.Storage)));
        }

        public QueueTypes.Models.NewPacket MapUplinkToQueueType(NewPacket receivedPacket)
        {
            return new QueueTypes.Models.NewPacket
            {
                Uplink = new QueueTypes.Models.ReceivedPackets.ReceivedPacket {
                    metadata = new QueueTypes.Models.ReceivedPackets.ReceivedPacketsMetadata {
                        Channel = receivedPacket.RxPacket.metadata.Channel,
                        CodingRate = receivedPacket.RxPacket.metadata.CodingRate,
                        DataRate = receivedPacket.RxPacket.metadata.DataRate,
                        Frequency = receivedPacket.RxPacket.metadata.Frequency,
                        LuminanceSignalToRatio = receivedPacket.RxPacket.metadata.LuminanceSignalToRatio,
                        Modulation = receivedPacket.RxPacket.metadata.Modulation,
                        RadioFrequencyChannel = receivedPacket.RxPacket.metadata.RadioFrequencyChannel,
                        RecievedSignalStrenghtIndicator = receivedPacket.RxPacket.metadata.RecievedSignalStrenghtIndicator,
                        Size = receivedPacket.RxPacket.metadata.Size,
                        Stat = receivedPacket.RxPacket.metadata.Stat,
                        Timestamp = receivedPacket.RxPacket.metadata.Timestamp,
                        Data = receivedPacket.RxPacket.metadata.Data
                    },
                    isRegistered = receivedPacket.RxPacket.isRegesteredDevice,
                    decodedPacket = new QueueTypes.Models.DecodedLoraPacket {
                        PhysicalPayload = new QueueTypes.Models.LoRaWanPacketSections.PhysicalPayload
                        {
                            MacHeader = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacHeader,
                            MIC = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MIC,
                            MacPayload = new QueueTypes.Models.LoRaWanPacketSections.MacPayload
                            {
                                FramePort = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacPayload.FramePort,
                                FramePayload = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacPayload.FramePayload,
                                FrameHeader = new QueueTypes.Models.LoRaWanPacketSections.FrameHeader
                                {
                                    DeviceAddress = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.DeviceAddress,
                                    FrameControlOctet = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameControlOctet,
                                    FrameCounter = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameCounter,
                                    FrameOptions = receivedPacket.RxPacket.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameOptions
                                }
                            }
                        },
                        OriginalHexString = receivedPacket.RxPacket.decodedPacket.OriginalHexString
                    }
                }        
            };
        }
    }
}
