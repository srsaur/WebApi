using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.INotifierServices
{
    public interface INotifier
    {
        IQueryable<string> OnlineUsers { get; }

        Task SendNotificationAsync(IReadOnlyList<string> userIds, object sendData);

        Task SendNotificationAsync(string userId, object sendData);

         Task SendNotificationToAllAsync(object data);

    }
}
