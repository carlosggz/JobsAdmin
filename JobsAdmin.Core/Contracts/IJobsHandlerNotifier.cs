using JobsAdmin.Core.Dtos;

namespace JobsAdmin.Core.Contracts
{
    public interface IJobsHandlerNotifier
    {
        void OnJobAdded(JobInfoDto jobInfo);
        void OnJobRemoved(string id);
        void OnJobProgress(NotificationDto notification);
    }
}