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
        IJobNotifier Notifier { get; set; }

        void DoWork();
    }
}
