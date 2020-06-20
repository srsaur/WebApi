using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Notification.INotifierServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext appDbContext;
        private readonly IMapper _mapper;
        private readonly INotificationHelper _hubContext;
        private readonly INotifier _notifier;

        public MessageService(AppDbContext appDb,IMapper mapper, INotificationHelper hubContext,INotifier notifier)
        {
            appDbContext = appDb;
            _mapper = mapper;
            _hubContext = hubContext;
            _notifier = notifier;
        }
        public async Task<IEnumerable<MessageDto>> GetMessages(string fromUser,string toUser)
        {
           return await appDbContext.Messages.Where(e =>
           (e.FromUserId == fromUser && e.ToUserId == toUser) 
           || (e.FromUserId == toUser && e.ToUserId == fromUser))
           .ProjectTo<MessageDto>(_mapper.ConfigurationProvider).OrderBy(e=>e.CreatedOn).ToListAsync();
        }

        public async Task SendMessage(MessageDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
            message.RelationKey = await RelationalKey(messageDto);
            await appDbContext.Messages.AddAsync(message);
            await appDbContext.SaveChangesAsync();
            await _hubContext.SendNotificationParaller(message.ToUserId, "GetNotify", _mapper.Map<MessageDto>(message));
            await _hubContext.SendNotificationParaller(message.FromUserId, "GetNotify", _mapper.Map<MessageDto>(message));
            await _notifier.SendNotificationAsync(message.ToUserId, null);
        }

        public async Task<dynamic> GetRecentChat(string fromUser)
        {
            var keys = await appDbContext.Messages.Where(e =>
           (e.FromUserId == fromUser || e.ToUserId == fromUser)).GroupBy(e => e.RelationKey, (msg, x) => x.Max(e=>e.Id)).ToListAsync();

           return await appDbContext.Messages.Where(e => keys.Contains(e.Id))
                .Include(e => e.ToUser)
                .Include(e => e.FromUser)
                .Select(e => new
                {
                    UserId= (e.FromUserId == fromUser) ? e.ToUser.Id : e.FromUser.Id,
                    UserName = (e.FromUserId == fromUser) ? e.ToUser.UserName : e.FromUser.UserName,
                    Image = (e.FromUserId == fromUser) ? e.ToUser.ImagePath : e.FromUser.ImagePath,
                    CreatedOn = e.MessageOn,
                    Message = e.Text
                }).OrderByDescending(e=>e.CreatedOn).ToListAsync();
        }

        private async Task<string> RelationalKey(MessageDto messageDto)
        {
           var relation =  await appDbContext.RelationKeys.FirstOrDefaultAsync(e =>
           (e.FromUserId == messageDto.FromUserId && e.ToUserId == messageDto.ToUserId)
           || (e.FromUserId == messageDto.ToUserId && e.ToUserId == messageDto.FromUserId));

            if (relation == null)
            {
              relation = (await appDbContext.RelationKeys.AddAsync(new RelationKeys { FromUserId = messageDto.FromUserId, ToUserId = messageDto.ToUserId })).Entity;       
            }

            return relation.Key;

        }
    }
}
