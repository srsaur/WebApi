using Microsoft.EntityFrameworkCore;
using Notification.INotifierServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.NotifierService
{
   internal class ConnectionManager : IConnectionManager
    {
       
        private readonly NotificationDbContext.NotificationDbContext _dbContext;

        public ConnectionManager(NotificationDbContext.NotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<string> OnlineUsers => _dbContext.NotificationConnections.GroupBy(e => e.UserId).Select(e => e.Key);

        public async Task AddConnection(string userName, string ConnectionId)
        {
            try
            {
               await _dbContext.NotificationConnections.AddAsync(new Model.NotificationConnection { ConnectionId = ConnectionId, UserId = userName });
               await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

        public async Task<IList<string>> GetConnection(string userName)
        {
           return await _dbContext.NotificationConnections.Where(e => e.UserId == userName).Select(e => e.ConnectionId).ToListAsync();
        }

        public async Task<bool> IsUserOnline(string userId)
        {
          return await  OnlineUsers.AnyAsync(e => e.Equals(userId));
        }

        public async Task RemoveConnection(string connectionId)
        {
            try
            {
                var connection= await  _dbContext.NotificationConnections.FirstOrDefaultAsync(e => e.ConnectionId == connectionId);
                if (connection != null)
                {
                    _dbContext.NotificationConnections.Remove(connection);
                   await _dbContext.SaveChangesAsync();
                }
            }
            catch{}
        }

    }
}
