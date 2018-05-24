using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace JobsAdmin.Web.Hub
{
    public class JobsHandlerNotifier : IJobsHandlerNotifier
    {
        private readonly IConnectionManager _connectionManager = null;
        private IHubConnectionContext<dynamic> Clients => _connectionManager.GetHubContext<JobsHub>().Clients;

        public JobsHandlerNotifier(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void OnJobAdded(JobInfoDto jobInfo)
        {
            Clients.All.addJob(jobInfo);
        }

        public void OnJobProgress(NotificationDto notification)
        {
            Clients.All.updateJob(notification);
        }

        public void OnJobRemoved(string id)
        {
            Clients.All.removeJob(id);
        }
    }
}