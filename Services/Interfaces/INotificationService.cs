namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message);
        Task<IEnumerable<AIHospitalManagementSys.Models.Domain.Notification>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
