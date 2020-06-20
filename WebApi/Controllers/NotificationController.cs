using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/notification")]
    [Authorize]
    public class NotificationsController:BaseController
    {
        private readonly INotificationService _notification;

        public NotificationsController(INotificationService notification)
        {
            _notification = notification;
        }

        [HttpGet("getNotificationList")]
        public async Task<IEnumerable<NotificationDto>> GetNotificationList(){
               return await _notification.GetNotificationList(UserId);
        }
        [HttpPost("insertNotification")]
        public async Task InsertNotification([FromBody]NotificationDto notificationDto){
              await _notification.InsertNotification(notificationDto);
        }
        
        [HttpGet("markAsRead")]
        public async Task MarkAsReadNotification(){
              await _notification.MarkAsReadNotification(UserId);
        }
    }
}