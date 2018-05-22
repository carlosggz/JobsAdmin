using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using Microsoft.AspNet.SignalR.Hubs;

namespace JobsAdmin.Web.Hub
{
    public class JobsHandlerNotifier : IJobsHandlerNotifier
    {
        private readonly IHubConnectionContext<dynamic> _clients;

        public JobsHandlerNotifier(IHubConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }        

        public void OnJobAdded(JobInfoDto jobInfo)
        {
            _clients.All.addJob(jobInfo);
        }

        public void OnJobProgress(NotificationDto notification)
        {
            _clients.All.updateJob(notification);
        }

        public void OnJobRemoved(string id)
        {
            _clients.All.removeJob(id);
        }
    }
}