using JobsAdmin.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Core.Dtos
{
    public class JobInfoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Progress { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? ScheduleAt { get; set; }
    }
}
