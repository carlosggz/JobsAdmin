using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsAdmin.Jobs
{
    public class SlowestJob : BaseJob
    {
        static readonly object _locker = new object();

        public SlowestJob()
            : base("Slowest Job")
        {}

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
                System.Threading.Thread.Sleep(1000);
            }

            lock (_locker)
            {
                Status = JobStatus.Finished;
            }

            SendNotification(NotificationType.Info, "Finished");
        } 
    }
}
