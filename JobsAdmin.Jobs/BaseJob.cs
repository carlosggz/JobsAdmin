using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Jobs
{
    public abstract class BaseJob: IJob
    {
        public BaseJob(string name)
        {
            Name = name;
        }

        #region IJob

        public string Id { get; private set; } = Guid.NewGuid().ToString();

        public string Name { get; private set; } = null;

        public int Progress { get; protected set; } = 0;

        public JobStatus Status { get; protected set; } = JobStatus.Waiting;

        public DateTime? LastActivity { get; private set; } = null;

        public string LastMessage { get; private set; } = null;

        public INotify Notify { get; set; }

        public abstract void DoWork();

        #endregion

        protected void SendNotification(NotificationType notificationType, string message)
        {
            Notify?.NotifyAction(new NotificationDto()
            {
                Id = this.Id,
                Message = message,
                NotificationType = notificationType,
                Progress = Progress,
                Status = Status
            });

            LastActivity = DateTime.Now;
            LastMessage = message;
        }
    }
}