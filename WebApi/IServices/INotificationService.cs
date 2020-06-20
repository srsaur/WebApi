using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.IServices
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationList(string userId);
        Task InsertNotification(NotificationDto notificationDto);
        Task MarkAsReadNotification(string userId);
    }
}