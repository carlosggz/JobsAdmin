using System;
using System.Collections.Generic;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;

namespace JobsAdmin.Core.Contracts
{
    public interface IJobsHandler
    {
        void AddJob(IJob job, int? recurrenceInMinutes = null);
        void AddDailyJob(IJob job, int hour);
        void AddWeeklyJob(IJob job, DayOfWeek dayOfWeek, int hour);
        void AddMonthlyJob(IJob job, int hour);

        IEnumerable<JobInfoDto> GetAllJobs();
        INotificationsBroker NotificationsBroker{ get; }
        ISchedulerHosting Hosting { get; set; }
    }
}