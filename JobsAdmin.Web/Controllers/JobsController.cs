using JobsAdmin.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JobsAdmin.Web.Hub;
using JobsAdmin.Handler;
using JobsAdmin.Core.Contracts;

namespace JobsAdmin.Web.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobsController : ApiController
    {
        public IJobsHandler Handler { get; set; }

        [HttpPost]
        [Route("AddNormal")]
        public void AddNormal()
        {
            Handler.AddJob(new NormalJob());
        }

        [HttpPost]
        [Route("AddSlowest")]
        public void AddSlowest()
        {
            Handler.AddJob(new SlowestJob());
        }

        [HttpPost]
        [Route("AddScheduled")]
        public void AddScheduled()
        {
            Handler.AddJob(new NormalJob(), TimeSpan.FromSeconds(30));
        }

        [HttpPost]
        [Route("Start/{id}")]
        public void StartJob(string id)
        {
            Handler.StartJob(id);
        }
    }
}