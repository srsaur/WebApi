
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.IServices;

namespace WebApi.Controllers
{
    [Route("api/Message")]
    [Authorize]
    public class MessageController:BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("GetMessages")]
        public async Task<IEnumerable<MessageDto>> GetMessages(string toUser)
        {
            return await _messageService.GetMessages(UserId, toUser);
        }

        [HttpPost("sendMessage")]
        public async  Task SendMessage([FromBody]MessageDto messageDto)
        {
            messageDto.FromUserId = UserId;
           await _messageService.SendMessage(messageDto);
        }

        [HttpGet("RecentChat")]
        public async Task<dynamic> GetRecentChat()
        {
          return await _messageService.GetRecentChat(UserId);
        }
    }
}
