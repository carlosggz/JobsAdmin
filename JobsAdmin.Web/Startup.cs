using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JobsAdmin.Jobs;
using JobsAdmin.Web.Hub;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JobsAdmin.Web.Startup))]

namespace JobsAdmin.Web
{
    public class Startup
    {
        private const int TimeIntervalSeconds = 30;
        private static readonly Timer _timer = new Timer(OnTimerElapsed);
        private static readonly JobScheduler _jobHost = new JobScheduler();

        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            //Scheduler
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(TimeIntervalSeconds));
        }

        private static void OnTimerElapsed(object sender)
        {
            _jobHost.DoWork(() => JobsHandler.Instance.ProcessSchedule());
        }
    }
}