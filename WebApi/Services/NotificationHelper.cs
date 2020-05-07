using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.IServices;

namespace WebApi.Services
{
    public class NotificationHelper : INotificationHelper
    {
        IHubContext<ChatHub> _hubContext;
        private readonly IConnectionManager _connectionManager;

        public NotificationHelper(IHubContext<ChatHub> hubContext,IConnectionManager connectionManager)
        {
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        public IEnumerable<string> OnlineUsers()
        {
           return _connectionManager.OnlineUsers;
        }

        public async Task SendNotificationParaller(string userName,string function,object message)
        {
            HashSet<string> connections = _connectionManager.GetConnection(userName);

            if(connections!=null && connections.Count > 0)
            {
                foreach (var connection in connections)
                {
                   await _hubContext.Clients.Clients(connection).SendAsync(function,message);
                }
            }
        }

        public void SendNotificationToAll()
        {
           
        }
    }
}
