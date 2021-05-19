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

        public async Task EnqueueToUplink(NewPacket packet)
        {
            var storagePacket = new QueueTypes.Queues.Uplink
            {
                Packet = MapUplinkToQueueType(packet)
            };

            _logger.Information("Publishing uplink notification {@Notification}", storagePacket);
            await _bus.PubSub.PublishAsync(storagePacket, handler => handler.WithTopic(nameof(QueueTypes.Queues.Uplink)));
        }

        public async Task EnqueueToStorage(NewPacket packet) 
        {
            var storagePacket = new QueueTypes.Queues.Storage { 
                Packet = MapUplinkToQueueType(packet)
            };

            _logger.Information("Publishing storage notification {@Notification}", storagePacket);
            await _bus.PubSub.PublishAsync(storagePacket, handler => handler.WithTopic(nameof(QueueTypes.Queues.Storage)));
        }

        public QueueTypes.Models.NewPacket MapUplinkToQueueType(NewPacket receivedPacket)
        {
            return new QueueTypes.Models.NewPacket
            {
                Uplink = new QueueTypes.Models.ReceivedPackets.ReceivedPacket {
                    metadata = new QueueTypes.Models.ReceivedPackets.ReceivedPacketsMetadata {
                        Channel = receivedPacket.Uplink.metadata.Channel,
                        CodingRate = receivedPacket.Uplink.metadata.CodingRate,
                        DataRate = receivedPacket.Uplink.metadata.DataRate,
                        Frequency = receivedPacket.Uplink.metadata.Frequency,
                        LuminanceSignalToRatio = receivedPacket.Uplink.metadata.LuminanceSignalToRatio,
                        Modulation = receivedPacket.Uplink.metadata.Modulation,
                        RadioFrequencyChannel = receivedPacket.Uplink.metadata.RadioFrequencyChannel,
                        RecievedSignalStrenghtIndicator = receivedPacket.Uplink.metadata.RecievedSignalStrenghtIndicator,
                        Size = receivedPacket.Uplink.metadata.Size,
                        Stat = receivedPacket.Uplink.metadata.Stat,
                        Timestamp = receivedPacket.Uplink.metadata.Timestamp,
                        Data = receivedPacket.Uplink.metadata.Data
                    },
                    isRegesteredDevice = receivedPacket.Uplink.isRegesteredDevice,
                    decodedPacket = new QueueTypes.Models.DecodedLoraPacket {
                        PhysicalPayload = new QueueTypes.Models.LoRaWanPacketSections.PhysicalPayload
                        {
                            MacHeader = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacHeader,
                            MIC = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MIC,
                            MacPayload = new QueueTypes.Models.LoRaWanPacketSections.MacPayload
                            {
                                FramePort = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacPayload.FramePort,
                                FramePayload = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacPayload.FramePayload,
                                FrameHeader = new QueueTypes.Models.LoRaWanPacketSections.FrameHeader
                                {
                                    DeviceAddress = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.DeviceAddress,
                                    FrameControlOctet = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameControlOctet,
                                    FrameCounter = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameCounter,
                                    FrameOptions = receivedPacket.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.FrameOptions
                                }
                            }
                        },
                        OriginalHexString = receivedPacket.Uplink.decodedPacket.OriginalHexString
                    }
                }        
            };
        }
    }
}
