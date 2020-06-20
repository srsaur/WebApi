using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.INotifierServices
{
    public interface IConnectionManager
    {
        /// <summary>
        /// Add UserConnection In SignalR
        /// </summary>
        /// <param name="userName">pass UserId for perticularUser</param>
        /// <param name="ConnectionId">pass ConnectionId for user</param>
        Task AddConnection(string userId,string ConnectionId);
        
        /// <summary>
        /// remove connection for ExistingUser
        /// </summary>
        /// <param name="connectionId">remove prticular user Device</param>
        Task RemoveConnection(string connectionId);

        /// <summary>
        /// get all connection for perticularUser
        /// </summary>
        /// <param name="userId">provide userId</param>
        /// <returns>List of connected device</returns>
        Task<IList<string>> GetConnection(string userId);

        /// <summary>
        /// Get All Online User
        /// </summary>
        IQueryable<string> OnlineUsers { get; }

        /// <summary>
        /// this function used for isUser online or Not
        /// </summary>
        /// <param name="userId">check user is online or not</param>
        /// <returns>User online return true otherwise false</returns>
        Task<bool> IsUserOnline(string userId);
    }
}
