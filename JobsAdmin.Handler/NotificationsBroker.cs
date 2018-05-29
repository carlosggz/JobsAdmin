using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsAdmin.Handler
{
    internal class NotificationsBroker : INotificationsBroker, IJobsHandlerNotifier
    {
        private readonly List<IJobsHandlerNotifier> _notifiers = new List<IJobsHandlerNotifier>();

        #region IJobsHandlerNotifier

        public void OnJobAdded(JobInfoDto jobInfo)
        {
            foreach (var notifier in _notifiers)
                notifier.OnJobAdded(jobInfo);
        }

        public void OnJobProgress(NotificationDto notification)
        {
            foreach (var notifier in _notifiers)
                notifier.OnJobProgress(notification);
        }

        public void OnJobRemoved(string id)
        {
            foreach (var notifier in _notifiers)
                notifier.OnJobRemoved(id);
        }

        #endregion

        #region INotificationsBroker

        public void Subscribe(IJobsHandlerNotifier notifier)
        {
            if (notifier != null && !_notifiers.Contains(notifier))
                _notifiers.Add(notifier);
        }

        public void Unsubscribe(IJobsHandlerNotifier notifier)
        {
            if (notifier != null &&_notifiers.Contains(notifier))
                _notifiers.Remove(notifier);
        }

        #endregion

    }
}
