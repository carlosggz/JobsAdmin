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

        public static JobInfoDto FromJob(IJob job)
        {
            return new JobInfoDto()
            {
                Id = job.Id,
                Name = job.Name,
                Progress = job.Progress,
                Status = job.Status,
                ScheduleAt = job.NextRunAt
            };
        }
    }
}
