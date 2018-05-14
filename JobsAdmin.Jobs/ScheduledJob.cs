using JobsAdmin.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Jobs
{
    public class ScheduledJob: BaseJob
    {
        static readonly object _locker = new object();

        public ScheduledJob()
            : base("Scheduled Job")
        { }

        public override void DoWork()
        {
            lock (_locker)
            {
                if (Status == JobStatus.InProgress)
                    return;

                Status = JobStatus.InProgress;
                Progress = 0;
            }

            SendNotification(NotificationType.Info, "Starting...");

            for (var index = 1; index <= 100; index++)
            {
                Progress = index;
                SendNotification(NotificationType.Info, $"Processing value {index}...");
                System.Threading.Thread.Sleep(100);
            }

            lock (_locker)
            {
                Status = JobStatus.Finished;
            }

            SendNotification(NotificationType.Info, "Finished");
        }
    }
}
