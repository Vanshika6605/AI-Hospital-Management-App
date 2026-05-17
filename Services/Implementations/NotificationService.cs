using AIHospitalManagementSys.Hubs;
using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepo;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IGenericRepository<Notification> notificationRepo, IHubContext<NotificationHub> hubContext)
        {
            _notificationRepo = notificationRepo;
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                ApplicationUserId = userId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveAsync();

            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _notificationRepo.GetAllAsync(filter: n => n.ApplicationUserId == userId);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepo.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _notificationRepo.Update(notification);
                await _notificationRepo.SaveAsync();
            }
        }
    }
}
