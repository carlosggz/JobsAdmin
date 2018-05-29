using System;
using System.Collections.Generic;
using System.Text;

namespace JobsAdmin.Core.Contracts
{
    public interface INotificationsBroker
    {
        void Subscribe(IJobsHandlerNotifier notifier);
        void Unsubscribe(IJobsHandlerNotifier notifier);
    }
}
