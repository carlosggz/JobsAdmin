using JobsAdmin.Core.Dtos;

namespace JobsAdmin.Core.Contracts
{
    public interface IJobNotifier
    {
        void NotifyAction(NotificationDto notification);
    }
}
