using Microsoft.AspNetCore.SignalR;
using Notification.INotifierServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.NotifierService
{
    public class Notifier : INotifier
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IHubContext<Notification> _context;

        public Notifier(IConnectionManager connectionManager,IHubContext<Notification> context)
        {
            _connectionManager = connectionManager;
            _context = context;
        }
        public IQueryable<string> OnlineUsers => _connectionManager.OnlineUsers;

        /// <summary>
        /// Notify All Client provied by  you
        /// </summary>
        /// <param name="userIds">provide user lists which you what to invoke</param>
        /// <param name="sendData">send data to the client</param>
        /// <returns>A System.Threading.Tasks.Task that represents the asynchronous invoke.</returns>
        public async Task SendNotificationAsync(IReadOnlyList<string> userIds,object sendData)
        {
             await _context.Clients.Groups(userIds).SendAsync("GetNotification", sendData);
        }
        /// <summary>
        /// Notify All Client provied by  you
        /// </summary>
        /// <param name="userIds">provide userId to invoke</param>
        /// <param name="sendData">send data to the client</param>
        /// <returns>A System.Threading.Tasks.Task that represents the asynchronous invoke.</returns>
        public async Task SendNotificationAsync(string userId, object sendData)
        {
            await _context.Clients.Groups(userId).SendAsync("GetNotification", sendData);
        }


        /// <summary>
        /// Notify All Client provied by  you
        /// </summary>
        /// <param name="sendData">send data to the Allclient</param>
        /// <returns>A System.Threading.Tasks.Task that represents the asynchronous invoke.</returns>

        public async Task SendNotificationToAllAsync(object data)
        {
            await _context.Clients.All.SendAsync("GetNotification",data);
        }

    }
}
