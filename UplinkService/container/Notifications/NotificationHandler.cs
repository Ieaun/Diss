namespace UplinkService.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    public class NotificationHandler : INotificationHandler<Notification>
    {
        private ILogger<NotificationHandler> logger;
        public NotificationHandler(ILogger<NotificationHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Recieved new notification :{@Notification}", notification);
            try
            {

            }
            catch (Exception e)
            {
                logger.LogInformation("Failed processing notification :{@Notification} with exception: {@Exception}", notification, e.Message);
                return Task.CompletedTask;
            }

            logger.LogInformation("Finished processing notification :{@Notification}", notification);
            return Task.CompletedTask;
        }
    }
}
