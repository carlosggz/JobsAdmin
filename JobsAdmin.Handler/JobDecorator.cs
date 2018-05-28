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

        static readonly object _locker = new object();
        private readonly IJob _job = null;

        #endregion

        public JobDecorator(IJob job, TimeSpan? recurrence = null)
        {
            _job = job ?? throw new InvalidParametersException();

            if (!recurrence.HasValue)
                return;

            RecurrencePeriod = recurrence;
            ReSchedule();
        }

        public JobStatus Status { get; private set; } = JobStatus.InQueued;
        public DateTime? LastActivity { get; private set; } = null;
        public string LastMessage { get; private set; } = null;
        public TimeSpan? RecurrencePeriod { get; private set; } = null;
        public DateTime? NextRunAt { get; private set; } = null;

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
                Status = RecurrencePeriod.HasValue ? JobStatus.Scheduled : JobStatus.ReadyToRemove;
            }

            if (RecurrencePeriod.HasValue)
            {
                ReSchedule();
                SendNotification(NotificationType.Info, "Re-scheduled");
            }
            else
                SendNotification(NotificationType.Info, "Finished");
        }

        #endregion

        #region Private

        private void ReSchedule()
        {
            NextRunAt = DateTime.Now.Add(RecurrencePeriod.Value);
            Status = JobStatus.Scheduled;
        }

        protected void SendNotification(NotificationType notificationType, string message, int? progress = null)
        {
            Notifier?.NotifyAction(new NotificationDto()
            {
                Id = this.Id,
                Message = message,
                NotificationType = notificationType,
                Progress = progress ?? Progress,
                Status = Status
            });

            LastActivity = DateTime.Now;
            LastMessage = message;
        }

        #endregion
    }
}
