using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using JobsAdmin.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Handler
{
    internal class JobDecorator: IJob
    {
        #region Private fields

        private readonly object _locker = new object();
        private readonly IJob _job = null;

        #endregion

        public JobDecorator(IJob job, Recurrence recurrence = null)
        {
            _job = job ?? throw new InvalidParametersException();
            RecurrencePeriod = recurrence;
            Status = recurrence == null ? JobStatus.InQueued : JobStatus.Scheduled;
        }

        public JobStatus Status { get; private set; } = JobStatus.InQueued;
        public DateTime? LastActivity { get; private set; } = null;
        public string LastMessage { get; private set; } = null;
        public Recurrence RecurrencePeriod { get; private set; } = null;

        #region IJob

        public IJobNotifier Notifier
        {
            get => _job.Notifier;
            set => _job.Notifier = value;
        }

        public string Id => _job.Id;

        public string Name => _job.Name;

        public int Progress => _job.Progress;

        public void DoWork()
        {
            lock (_locker)
            {
                if (Status == JobStatus.InProgress)
                    return;

                Status = JobStatus.InProgress;
            }

            try
            {
                _job.DoWork();
            }
            catch (Exception ex)
            {
                SendNotification(NotificationType.Error, $"Error running job: {ex.Message}");
            }           

            lock (_locker)
            {
                Status = RecurrencePeriod != null ? JobStatus.Scheduled : JobStatus.ReadyToRemove;
            }

            if (RecurrencePeriod != null)
            {
                RecurrencePeriod.ReSchedule();
                SendNotification(NotificationType.Info, "Re-scheduled at " + RecurrencePeriod.NextRunAt.ToString("MM/dd/yyyy H:mm"));
            }
            else
                SendNotification(NotificationType.Info, "Finished");
        }

        #endregion

        #region Private

        protected void SendNotification(NotificationType notificationType, string message, int? progress = null)
        {
            Notifier?.NotifyAction(new NotificationDto()
            {
                Id = this.Id,
                Message = message,
                NotificationType = notificationType,
                Progress = progress ?? Progress,
                Status = Status,
                ScheduledAt = Status == JobStatus.Scheduled ? (DateTime?)RecurrencePeriod.NextRunAt : null
            });

            LastActivity = DateTime.Now;
            LastMessage = message;
        }

        #endregion
    }
}
