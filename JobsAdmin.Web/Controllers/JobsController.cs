using JobsAdmin.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JobsAdmin.Web.Hub;

namespace JobsAdmin.Web.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobsController : ApiController
    {
        [HttpPost]
        [Route("AddNormal")]
        public void AddNormal()
        {
            JobsHandler.Instance.AddJob(new NormalJob());
        }

        [HttpPost]
        [Route("AddSlowest")]
        public void AddSlowest()
        {
            JobsHandler.Instance.AddJob(new SlowestJob());
        }

        [HttpPost]
        [Route("Start/{id}")]
        public void StartJob(string id)
        {
            JobsHandler.Instance.StartJob(id);
        }
    }
}