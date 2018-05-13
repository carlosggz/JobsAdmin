using JobsAdmin.Core.Contracts;

namespace JobsAdmin.Jobs
{
    public class NormalJob : BaseJob
    {
        static readonly object _locker = new object();

        public NormalJob()
            : base("Normal Job")
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
                System.Threading.Thread.Sleep(500);
            }

            lock (_locker)
            {
                Status = JobStatus.Finished;
            }

            SendNotification(NotificationType.Info, "Finished");
        }
    }
}
