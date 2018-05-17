using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsAdmin.Core.Contracts
{
    public interface IJob
    {
        string Id { get; }
        string Name { get; }
        int Progress { get; }
        JobStatus Status { get; }
        DateTime? LastActivity { get; }
        string LastMessage { get; }
        INotify Notify { get; set; }
        TimeSpan? RecurrencePeriod { get; }
        DateTime? NextRunAt { get; }

        void DoWork();
    }
}
