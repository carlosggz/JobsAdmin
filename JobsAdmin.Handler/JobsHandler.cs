﻿using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobsAdmin.Handler
{
    public class JobsHandler : IJobsHandler, IJobNotifier
    {
        #region Constants

        private const int SchedulerTimeInSeconds = 30;
        private const int MaxJobsRunning = 3;
        
        #endregion

        #region Singleton Methods

        private readonly static Lazy<JobsHandler> _instance = new Lazy<JobsHandler>(() => new JobsHandler());

        private JobsHandler()
        {
            _timer = new Timer(ProcessSchedule);
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(SchedulerTimeInSeconds));

            _notificationsBroker = new NotificationsBroker();
        }

        public static JobsHandler Instance => _instance.Value;

        #endregion

        #region Private

        private readonly ConcurrentDictionary<string, JobDecorator> _jobs = new ConcurrentDictionary<string, JobDecorator>();
        private readonly Timer _timer = null;
        private readonly NotificationsBroker _notificationsBroker = null;
        private static readonly object _locker = new object();

        private void RemoveJob(string id)
        {
            if (!_jobs.TryRemove(id, out JobDecorator job))
                return;

            job.Notifier = null;
            _notificationsBroker.OnJobRemoved(id);
        }

        private static JobInfoDto FromJob(JobDecorator job)
        {
            return new JobInfoDto()
            {
                Id = job.Id,
                Name = job.Name,
                Progress = job.Progress,
                Status = job.Status,
                ScheduledAt = job.RecurrencePeriod?.NextRunAt
            };
        }

        private void ProcessSchedule(object sender)
        {
            if (this.Hosting == null)
                return;

            lock (_locker)
            {
                this.Hosting.DoWork(() =>
                {
                    var runningJobs = GetRunningJobsCount();

                    if (runningJobs >= MaxJobsRunning)
                        return;

                    foreach (var job in GetJobsTuRun(MaxJobsRunning - runningJobs))
                        new Task(() => job.DoWork()).Start();
                });
            }
        }

        private void StartJob(string id)
        {
            if (!_jobs.ContainsKey(id))
                return;

            _jobs[id].DoWork();
        }

        private int GetRunningJobsCount() => _jobs.Count(x => x.Value.Status == JobStatus.InProgress);

        private IEnumerable<JobDecorator> GetJobsTuRun(int max)
        {
            var toRun = new List<JobDecorator>();
            toRun.AddRange(_jobs.Values.Where(x => x.Status == JobStatus.Scheduled && x.RecurrencePeriod.NextRunAt <= DateTime.Now));
            toRun.AddRange(_jobs.Values.Where(x => x.Status == JobStatus.InQueued));
            return toRun.Take(max);
        }

        private void EnQueueJob(IJob job, Recurrence recurrence)
        {
            if (job == null)
                return;

            var decoratedJob = new JobDecorator(job, recurrence);
            _jobs[decoratedJob.Id] = decoratedJob;
            decoratedJob.Notifier = this;
            _notificationsBroker.OnJobAdded(FromJob(decoratedJob));
        }

        #endregion

        #region IJobNotifier

        public void NotifyAction(NotificationDto notification)
        {
            if (notification.Status == JobStatus.ReadyToRemove)
                RemoveJob(notification.Id);
            else
                _notificationsBroker.OnJobProgress(notification);
        }

        #endregion

        #region IJobsHandler

        public INotificationsBroker NotificationsBroker => _notificationsBroker;

        public ISchedulerHosting Hosting { get; set; }

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return Instance
                ._jobs
                .Values
                .Select(FromJob)
                .ToList();
        }

        public void AddJob(IJob job, int? recurrenceInMinutes = null)
        {
            EnQueueJob(job, !recurrenceInMinutes.HasValue ? null : Recurrence.BuildFromMinutes(recurrenceInMinutes.Value));
        }

        public void AddDailyJob(IJob job, int hour)
        {
            EnQueueJob(job, Recurrence.BuildDaily(hour));
        }

        public void AddWeeklyJob(IJob job, DayOfWeek dayOfWeek, int hour)
        {
            EnQueueJob(job, Recurrence.BuildWeekly(dayOfWeek, hour));
        }

        public void AddMonthlyJob(IJob job, int hour)
        {
            EnQueueJob(job, Recurrence.BuildMonthly(hour));
        }

        #endregion
    }
}