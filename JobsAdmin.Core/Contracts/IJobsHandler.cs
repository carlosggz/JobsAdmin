using System;
using System.Collections.Generic;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;

namespace JobsAdmin.Core.Contracts
{
    public interface IJobsHandler
    {
        void AddJob(IJob job, TimeSpan? recurrence = null);
        IEnumerable<JobInfoDto> GetAllJobs();
        INotificationsBroker NotificationsBroker{ get; }
        ISchedulerHosting Hosting { get; set; }
    }
}