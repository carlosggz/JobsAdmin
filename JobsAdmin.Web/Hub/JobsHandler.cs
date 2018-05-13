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

        private readonly object _updateStockPricesLock = new object();

        private JobsHandler(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        private JobInfoDto ToDto(IJob job) => new JobInfoDto() { Id = job.Id, Name = job.Name, Progress = job.Progress, Status = job.Status };

        public static JobsHandler Instance => _instance.Value;

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return Instance
                ._jobs
                .Values
                .Select(x => ToDto(x))
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
            Clients.All.addJob(ToDto(job));
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
    }
}