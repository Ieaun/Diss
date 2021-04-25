namespace UplinkService.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    public class Notification: INotification
    {
        public int Id { get; set; }
        public object Payload { get; set; }
    }
}
