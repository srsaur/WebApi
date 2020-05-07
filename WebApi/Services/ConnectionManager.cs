using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IServices;

namespace WebApi.Services
{
    public class ConnectionManager : IConnectionManager
    {
        private static Dictionary<string, HashSet<string>> userMaps = new Dictionary<string, HashSet<string>>();

        public IEnumerable<string> OnlineUsers => userMaps.Keys;

        public void AddConnection(string userName, string ConnectionId)
        {
            lock (userMaps)
            {
                if (!userMaps.ContainsKey(userName))
                {
                    userMaps[userName] = new HashSet<string>();
                }
                userMaps[userName].Add(ConnectionId);
            }
        }

        public HashSet<string> GetConnection(string userName)
        {
            lock (userMaps)
            {
                return userMaps.GetValueOrDefault(userName);
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (userMaps)
            {
                foreach (var userName in userMaps.Keys)
                {
                    if (userMaps[userName].Contains(connectionId))
                    {
                        userMaps[userName].Remove(connectionId);

                        if (!userMaps[userName].Any())
                        {
                            userMaps.Remove(userName);
                        }
                        break;
                    }
                }
            }
        }
    }
}
