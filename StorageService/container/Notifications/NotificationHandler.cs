namespace StorageService.Notifications
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using StorageService.Database;

    public class NotificationHandler : INotificationHandler<Notification>
    {
        private ILogger<NotificationHandler> _logger;
        private IDatabase _database;
        public NotificationHandler(ILogger<NotificationHandler> logger, IDatabase database)
        {
            this._logger = logger;
            this._database = database;
        }

        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received new notification :{@Notification}", notification);
            try
            {
                _database.Create(new StubObject());
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
