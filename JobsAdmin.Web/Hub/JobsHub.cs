using System.Collections.Generic;
using JobsAdmin.Core.Dtos;
using Microsoft.AspNet.SignalR.Hubs;

namespace JobsAdmin.Web.Hub
{
    [HubName("jobsAdminHub")]
    public class JobsHub : HubBase
    {
        private readonly JobsHandler _handler;

        public JobsHub() 
            : this(JobsHandler.Instance)
        { }

        public JobsHub(JobsHandler handler)
        {
            _handler = handler;
        }

        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return _handler.GetAllJobs();
        }
    }
}