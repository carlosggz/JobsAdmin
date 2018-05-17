using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using JobsAdmin.Jobs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace JobsAdmin.Web.Hub
{
    public class JobsHandler: INotify
    {
        private readonly static Lazy<JobsHandler> _instance = new Lazy<JobsHandler>(() => new JobsHandler(GlobalHost.ConnectionManager.GetHubContext<JobsHub>().Clients));

        private readonly ConcurrentDictionary<string, IJob> _jobs = new ConcurrentDictionary<string, IJob>();

        static readonly object _locker = new object();

        private JobsHandler(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public static JobsHandler Instance => _instance.Value;

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return Instance
                ._jobs
                .Values
                .Select(JobInfoDto.FromJob)
                .ToList();
        }

        public void NotifyAction(NotificationDto notification)
        {
            Clients.All.updateJob(notification);    
        }

        public void AddJob(IJob job)
        {
            _jobs[job.Id] = job;
            job.Notify = this;
            Clients.All.addJob(JobInfoDto.FromJob(job));
        }

        public void RemoveJob(string id)
        {
            if (!_jobs.TryRemove(id, out IJob job))
                return;

            job.Notify = null;
            Clients.All.removeJob(id);
        }

        public int InstancesOfJob(Type type)
        {
            return _jobs.Count(x => x.Value.GetType() == type);
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
    }
}