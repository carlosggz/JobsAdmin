using System.Collections.Generic;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace JobsAdmin.Web.Hub
{
    [HubName("jobsAdminHub")]
    public class JobsHub : HubBase
    {
        private readonly IJobsHandler _handler;

        public JobsHub()
        {
            _handler = JobsAdmin.Handler.JobsHandler.Instance;
            _handler.Notifier = new JobsHandlerNotifier(GlobalHost.ConnectionManager.GetHubContext<JobsHub>().Clients);
        }

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return _handler.GetAllJobs();
        }
    }
}