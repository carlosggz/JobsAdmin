using JobsAdmin.Core.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobsAdmin.WebDotNetCore.Hub
{
    public class JobScheduler : ISchedulerHosting, IWebHost
    {
        private IWebHost _webHost = null;

        public JobScheduler(IWebHost webHost)
        {
            _webHost = webHost;
        }

        private void SetShuttingDownValue(bool newValue)
        {
            lock (_lock)
            {
                _shuttingDown = newValue;
            }
        }

        #region ISchedulerHosting

        private readonly object _lock = new object();
        private bool _shuttingDown = false;

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

        #endregion

        #region IWebHost

        public IFeatureCollection ServerFeatures => _webHost.ServerFeatures;

        public IServiceProvider Services => _webHost.Services;

        public void Dispose() => _webHost.Dispose();

        public void Start()
        {
            SetShuttingDownValue(false);
            _webHost.Start();
        }

        public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetShuttingDownValue(false);
            return _webHost.StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetShuttingDownValue(true);
            return _webHost.StopAsync(cancellationToken);
        }

        #endregion
    }
}
