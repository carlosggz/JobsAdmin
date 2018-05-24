using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JobsAdmin.WebDotNetCore.Hub
{
    public class JobsHandlerNotifier : IJobsHandlerNotifier
    {
        private readonly IServiceProvider _serviceProvider = null;
        private IHubClients Clients => Get<IHubContext<JobsHub>>().Clients;

        public JobsHandlerNotifier(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private T Get<T>() where T: class
        {
            return _serviceProvider.GetService(typeof(T)) as T;
        }

        public void OnJobAdded(JobInfoDto jobInfo)
        {
            Clients.All.SendAsync("addJob", jobInfo);
        }

        public void OnJobProgress(NotificationDto notification)
        {
            Clients.All.SendAsync("updateJob", notification);
        }

        public void OnJobRemoved(string id)
        {
            Clients.All.SendAsync("removeJob", id);
        }
    }
}