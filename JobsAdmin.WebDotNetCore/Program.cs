using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JobsAdmin.Core.Contracts;
using JobsAdmin.WebDotNetCore.Hub;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JobsAdmin.WebDotNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webhost = BuildWebHost(args);
            var handler = webhost.Services.GetService(typeof(IJobsHandler)) as IJobsHandler;
            handler.Hosting = new JobScheduler(webhost);
            webhost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
