using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Notification.INotifierServices;

namespace Notification
{
    public class Notification : Hub
    {
        private readonly IConnectionManager _connectionManager;

        public Notification(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task ConnectionAddAsync(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await _connectionManager.AddConnection(userId, Context.ConnectionId);
        }

        public async Task ConnectionRemoveAsync(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _connectionManager.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
