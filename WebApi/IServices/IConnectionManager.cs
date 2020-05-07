using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IServices
{
    public interface IConnectionManager
    {
        /// <summary>
        /// Add UserConnection In SignalR
        /// </summary>
        /// <param name="userName">pass UserName for perticularUser</param>
        /// <param name="ConnectionId">pass ConnectionId for user</param>
        void AddConnection(string userName,string ConnectionId);
        
        /// <summary>
        /// remove connection for ExistingUser
        /// </summary>
        /// <param name="connectionId"></param>
        void RemoveConnection(string connectionId);

        /// <summary>
        /// get all connection for perticularUser
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        HashSet<string> GetConnection(string userName);

        /// <summary>
        /// Get All Online User
        /// </summary>
        IEnumerable<string> OnlineUsers { get; }
    }
}
