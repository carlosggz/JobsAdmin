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
        #region Private fields

        static readonly object _locker = new object();

        #endregion

        #region Constructor

        protected BaseJob(string name, TimeSpan? recurrence = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidParametersException("Invalid job name");

            Name = name;

            if (!recurrence.HasValue)
                return;

            RecurrencePeriod = recurrence;
            ReSchedule();
        }

        #endregion

        #region IJob

        public string Id { get; private set; } = Guid.NewGuid().ToString();

        public string Name { get; private set; } = null;

        public int Progress { get; protected set; } = 0;

        public JobStatus Status { get; protected set; } = JobStatus.Waiting;

        public DateTime? LastActivity { get; private set; } = null;

        public string LastMessage { get; private set; } = null;

        public INotify Notify { get; set; }

        public TimeSpan? RecurrencePeriod { get; private set; } = null;

        public DateTime? NextRunAt { get; private set; } = null;

        public void DoWork()
        {
            lock (_locker)
            {
                if (Status == JobStatus.InProgress)
                    return;

                Status = JobStatus.InProgress;
                Progress = 0;
            }

            SendNotification(NotificationType.Info, "Starting...");

            Execute();

            lock (_locker)
            {
                Status = JobStatus.Finished;
            }

            SendNotification(NotificationType.Info, "Finished");

            if (RecurrencePeriod.HasValue)
            {
                ReSchedule();
                SendNotification(NotificationType.Info, "Re-scheduled");
            }
        }

        #endregion

        #region Protected

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

        protected abstract void Execute();

        #endregion

        #region Private

        private void ReSchedule()
        {
            NextRunAt = DateTime.Now.Add(RecurrencePeriod.Value);
            Status = JobStatus.Scheduled;
        }

        #endregion
    }
}