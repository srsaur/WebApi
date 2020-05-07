using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.IServices
{
    public interface INotificationHelper
    {
        void SendNotificationToAll();

        IEnumerable<string> OnlineUsers();

        Task SendNotificationParaller(string userName,string function,object messageDto);
    }
}
