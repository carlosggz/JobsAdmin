using JobsAdmin.Core.Contracts;
using System;

namespace JobsAdmin.Core.Dtos
{
    public class NotificationDto
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public int Progress { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }
}
