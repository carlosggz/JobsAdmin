using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Jobs;
using JobsAdmin.WebDotNetCore.Hub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JobsAdmin.WebDotNetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Jobs")]
    public class JobsController : Controller
    {
        private IJobsHandler _handler = null;

        public JobsController(IJobsHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [Route("AddNormal")]
        public void AddNormal()
        {
            _handler.AddJob(new NormalJob());
        }

        [HttpPost]
        [Route("AddSlowest")]
        public void AddSlowest()
        {
            _handler.AddJob(new SlowestJob());
        }

        [HttpPost]
        [Route("AddScheduled")]
        public void AddScheduled()
        {
            _handler.AddJob(new NormalJob(), TimeSpan.FromSeconds(30));
        }

        [HttpPost]
        [Route("Start/{id}")]
        public void StartJob(string id)
        {
            _handler.StartJob(id);
        }
    }
}