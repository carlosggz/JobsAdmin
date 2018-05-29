using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Handler
{
    internal class NotificationsBroker : INotificationsBroker, IJobsHandlerNotifier
    {
        private readonly List<IJobsHandlerNotifier> _notifiers = new List<IJobsHandlerNotifier>();
        private readonly object _locker = new object();

        #region IJobsHandlerNotifier

        public void OnJobAdded(JobInfoDto jobInfo)
        {
            lock (_locker)
            {
                foreach (var notifier in _notifiers)
                    Task.Run(() => notifier.OnJobAdded(jobInfo));
            }
        }

        public void OnJobProgress(NotificationDto notification)
        {
            lock (_locker)
            {
                foreach (var notifier in _notifiers)
                    Task.Run(() => notifier.OnJobProgress(notification));
            }
        }

        public void OnJobRemoved(string id)
        {
            lock (_locker)
            {
                foreach (var notifier in _notifiers)
                    Task.Run(() => notifier.OnJobRemoved(id));
            }
        }

        #endregion

        #region INotificationsBroker

        public void Subscribe(IJobsHandlerNotifier notifier)
        {
            lock (_locker)
            {
                if (notifier != null && !_notifiers.Contains(notifier))
                    _notifiers.Add(notifier);
            }
        }

        public void Unsubscribe(IJobsHandlerNotifier notifier)
        {
            lock (_locker)
            {
                if (notifier != null && _notifiers.Contains(notifier))
                    _notifiers.Remove(notifier);
            }
        }

        #endregion

    }
}
