using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Handler
{
    public class JobsHandler : IJobsHandler, IJobNotifier
    {
        #region Singleton Methods

        private readonly static Lazy<JobsHandler> _instance = new Lazy<JobsHandler>(() => new JobsHandler());

        private JobsHandler()
        { }

        public static JobsHandler Instance => _instance.Value;

        #endregion

        #region Private

        private readonly ConcurrentDictionary<string, JobDecorator> _jobs = new ConcurrentDictionary<string, JobDecorator>();

        private static readonly object _locker = new object();

        private void RemoveJob(string id)
        {
            if (!_jobs.TryRemove(id, out JobDecorator job))
                return;

            job.Notifier = null;
            Notifier?.OnJobRemoved(id);
        }

        private static JobInfoDto FromJob(JobDecorator job)
        {
            return new JobInfoDto()
            {
                Id = job.Id,
                Name = job.Name,
                Progress = job.Progress,
                Status = job.Status,
                ScheduleAt = job.NextRunAt
            };
        }

        #endregion

        #region IJobNotifier

        public void NotifyAction(NotificationDto notification)
        {
            if (notification.Status == JobStatus.ReadyToRemove)
                RemoveJob(notification.Id);
            else
                Notifier?.OnJobProgress(notification);
        }

        #endregion

        #region IJobsHandler

        public IJobsHandlerNotifier Notifier { get; set; }

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return Instance
                ._jobs
                .Values
                .Select(FromJob)
                .ToList();
        }

        public void AddJob(IJob job, TimeSpan? recurrence = null)
        {
            var decoratedJob = new JobDecorator(job, recurrence);
            _jobs[decoratedJob.Id] = decoratedJob;
            decoratedJob.Notifier = this;
            Notifier?.OnJobAdded(FromJob(decoratedJob));
        }

        public void StartJob(string id)
        {
            if (!_jobs.ContainsKey(id))
                return;

            _jobs[id].DoWork();
        }

        public void ProcessSchedule()
        {
            lock (_locker)
            {
                var jobsToRun = _jobs
                    .Values
                    .Where(x => x.Status == JobStatus.Scheduled && x.NextRunAt.HasValue && x.NextRunAt.Value <= DateTime.Now);

                foreach (var job in jobsToRun)
                    job.DoWork();
            }
        }

        #endregion
    }
}