using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Core.Contracts
{
    public interface ISchedulerHosting
    {
        void DoWork(Action work);
    }
}
