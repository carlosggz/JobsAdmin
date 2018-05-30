using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Jobs;
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
            handler.NotificationsBroker.Subscribe(new SignalRNotifier(webhost.Services));
            handler.NotificationsBroker.Subscribe(new EmailNotifier());
            handler.NotificationsBroker.Subscribe(new LoggingNotifier());
            handler.AddJob(new NormalJob(), 60);
            handler.AddDailyJob(new NormalJob(), 23);
            handler.AddWeeklyJob(new NormalJob(), DayOfWeek.Monday, 21);
            handler.AddMonthlyJob(new NormalJob(), 0);

            webhost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
    }
}
