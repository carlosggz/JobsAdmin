using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsAdmin.WebDotNetCore.Hub
{
    public class EmailNotifier : IJobsHandlerNotifier
    {
        public void OnJobAdded(JobInfoDto jobInfo)
        {
            //Nothing
        }

        public void OnJobProgress(NotificationDto notification)
        {
            if (notification.Status == JobStatus.Finished)
            {
                //Send email
            }
        }

        public void OnJobRemoved(string id)
        {
            //Nothing
        }
    }
}