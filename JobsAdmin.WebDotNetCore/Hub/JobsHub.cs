using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using JobsAdmin.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace JobsAdmin.WebDotNetCore.Hub
{
    public class JobsHub: Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IJobsHandler _handler;

        public JobsHub(IJobsHandler handler)
        {
            _handler = handler;
        }

        [HubMethodName("getAllJobs")]
        public IEnumerable<JobInfoDto> GetAllJobs()
        {
            return _handler.GetAllJobs();
        }
    }
}
