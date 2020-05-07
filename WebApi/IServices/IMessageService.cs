using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.IServices
{
    public interface IMessageService
    {
         Task<IEnumerable<MessageDto>> GetMessages(string fromUser, string toUser);
         Task SendMessage(MessageDto messageDto);
        Task<dynamic> GetRecentChat(string fromUser);
    }
}
