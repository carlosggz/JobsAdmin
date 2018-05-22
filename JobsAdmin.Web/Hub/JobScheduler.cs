using JobsAdmin.Core.Contracts;
using System;
using System.Web.Hosting;

namespace JobsAdmin.Web.Hub
{
    public class JobScheduler : IRegisteredObject, ISchedulerHosting
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;

        public JobScheduler()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }

        public void DoWork(Action work)
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                work();
            }
        }
    }
}