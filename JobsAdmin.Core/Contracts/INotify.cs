using JobsAdmin.Core.Dtos;

namespace JobsAdmin.Core.Contracts
{
    public interface INotify
    {
        void NotifyAction(NotificationDto notification);
    }
}
