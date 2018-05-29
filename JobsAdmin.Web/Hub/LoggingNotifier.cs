using JobsAdmin.Core.Contracts;
using JobsAdmin.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsAdmin.Web.Hub
{
    public class LoggingNotifier : IJobsHandlerNotifier
    {
        public void OnJobAdded(JobInfoDto jobInfo)
        {
            //Add event to the database
        }

        public void OnJobProgress(NotificationDto notification)
        {
            //Add event to the database
        }

        public void OnJobRemoved(string id)
        {
            //Nothing
        }
    }
}