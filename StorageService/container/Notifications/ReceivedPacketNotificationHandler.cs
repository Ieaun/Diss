namespace StorageService.Notifications.ReceivedPacketNotifications
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using StorageService.Database;

    public class ReceivedPacketNotificationHandler : INotificationHandler<NewPacketNotification>
    {
        private ILogger<ReceivedPacketNotificationHandler> _logger;
        private IDatabase _database;
        public ReceivedPacketNotificationHandler(ILogger<ReceivedPacketNotificationHandler> logger, IDatabase database)
        {
            this._logger = logger;
            this._database = database;
        }

        public Task Handle(NewPacketNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received new notification :{@Notification}", notification);
            try
            {
                _database.Create(notification.Packet);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Failed storing notification :{@Notification} with exception: {@Exception}", notification, e.Message);
                return Task.CompletedTask;
            }

            _logger.LogInformation("Finished storing notification :{@Notification}", notification);
            return Task.CompletedTask;
        }
    }
}
