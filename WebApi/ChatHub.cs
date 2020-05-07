using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IConnectionManager _connectionManager;
        private readonly INotificationHelper notificationHelper;
        private readonly IFriendRequestService _friendRequest;

        public string userId
        {
            get
            {
                return Context.User.FindFirst("sid").Value;
            }
        }

        public ChatHub(IConnectionManager connectionManager, INotificationHelper notificationHelper,IFriendRequestService friendRequest)
        {
            _connectionManager = connectionManager;
            this.notificationHelper = notificationHelper;
            _friendRequest = friendRequest;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task<string> GetConnectionId()
        {
            _connectionManager.AddConnection(userId, Context.ConnectionId);
            await RefreshFriendRequestList();
            return Context.ConnectionId;
        }

        public async Task<IEnumerable<string>> GetOnlineUsers()
        {
            var friends =await _friendRequest.GetFriendIds(userId);
            return _connectionManager.OnlineUsers.Where(e=> friends.Contains(e));
        }

        public void notify(string userName)
        {
            notificationHelper.SendNotificationParaller(userName, "typing", userId);
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
