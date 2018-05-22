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
        void StartJob(string id);
        IJobsHandlerNotifier Notifier { get; set; }
        ISchedulerHosting Hosting { get; set; }
    }
}