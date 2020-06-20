using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notification.INotifierServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IServices;


namespace WebApi
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IServices.IConnectionManager _connectionManager;
        private readonly INotificationHelper notificationHelper;
        private readonly IFriendRequestService _friendRequest;
        private readonly INotificationService notificationService;

        public string userId
        {
            get
            {
                return Context.User.FindFirst("sid").Value;
            }
        }

        public ChatHub(IServices.IConnectionManager connectionManager, INotificationHelper notificationHelper, IFriendRequestService friendRequest, INotificationService notificationService)
        {
            _connectionManager = connectionManager;
            this.notificationHelper = notificationHelper;
            _friendRequest = friendRequest;
            this.notificationService = notificationService;
        }

        public async override Task OnConnectedAsync()
        {
           await base.OnConnectedAsync();
            await RefreshFriendRequestList();
           _connectionManager.AddConnection(userId, Context.ConnectionId);
        }

        public async Task<IEnumerable<string>> GetOnlineUsers()
        {
            var friends = await _friendRequest.GetFriendIds(userId);
            return _connectionManager.OnlineUsers.Where(e => friends.Contains(e));
        }

        public void notify(string userName)
        {
            notificationHelper.SendNotificationParaller(userName, "typing", userId);
        }

        public async Task<IEnumerable<WebApi.Dtos.NotificationDto>> GetNotifcationsAsync()
        {
            return await notificationService.GetNotificationList(userId);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionManager.RemoveConnection(Context.ConnectionId);
            await RefreshFriendRequestList();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task RefreshFriendRequestList()
        {
            foreach (var item in await GetOnlineUsers())
            {
                await notificationHelper.SendNotificationParaller(item, "updateOnlineUser", null);
            }
        }
        
    }
}
