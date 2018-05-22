using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using JobsAdmin.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Jobs
{
    public abstract class BaseJob: IJob
    {
        #region Constructor

        protected BaseJob(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidParametersException("Invalid job name");

            Name = name;
        }

        #endregion

        #region IJob

        public string Id { get; private set; } = Guid.NewGuid().ToString();

        public string Name { get; private set; } = null;

        public int Progress { get; protected set; } = 0;

        public IJobNotifier Notifier { get; set; }

        public void DoWork()
        {
            Progress = 0;

            SendNotification(NotificationType.Info, "Starting...");

            Execute();

            Progress = 100;
            SendNotification(NotificationType.Info, "Finished");
        }

        #endregion

        #region Protected

        protected abstract void Execute();

        protected void SendNotification(NotificationType notificationType, string message)
        {
            Notifier?.NotifyAction(new NotificationDto()
            {
                Id = Id,
                Message = message,
                NotificationType = notificationType,
                Progress = Progress,
                Status = JobStatus.InProgress
            });
        }

        #endregion
    }
}