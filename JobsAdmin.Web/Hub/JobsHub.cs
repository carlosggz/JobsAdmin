using System.Collections.Generic;
using Autofac;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace JobsAdmin.Web.Hub
{
    [HubName("jobsAdminHub")]
    public class JobsHub : HubBase
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        private readonly IJobsHandler _handler;

        public JobsHub(ILifetimeScope lifetimeScope)
        {
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();
            _handler = _hubLifetimeScope.Resolve<IJobsHandler>();
            _handler.Notifier = new JobsHandlerNotifier(GlobalHost.ConnectionManager.GetHubContext<JobsHub>().Clients);
        }

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return _handler.GetAllJobs();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}