using JobsAdmin.Core.Contracts;
using System;

namespace JobsAdmin.Jobs
{
    public class NormalJob : BaseJob
    {
        public NormalJob()
            : base("Normal Job")
        {}

        protected override void Execute()
        {
            for (var index = 1; index <= 100; index++)
            {
                Progress = index;
                SendNotification(NotificationType.Info, $"Processing value {index}...");
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
