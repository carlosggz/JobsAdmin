using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsAdmin.Jobs
{
    public class SlowestJob : BaseJob
    {
        public SlowestJob()
            : base("Slowest Job")
        {}

        protected override void Execute()
        {
            for (var index = 1; index <= 100; index++)
            {
                Progress = index;
                SendNotification(NotificationType.Info, $"Processing value {index}...");
                System.Threading.Thread.Sleep(1000);
            }
        } 
    }
}
