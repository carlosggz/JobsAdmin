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
        private readonly IHubClients _clients;

        public JobsHandlerNotifier(IHubClients clients)
        {
            _clients = clients;
        }

        public void OnJobAdded(JobInfoDto jobInfo)
        {
            _clients.All.SendAsync("addJob", jobInfo);
        }

        public void OnJobProgress(NotificationDto notification)
        {
            _clients.All.SendAsync("updateJob", notification);
        }

        public void OnJobRemoved(string id)
        {
            _clients.All.SendAsync("removeJob", id);
        }
    }
}