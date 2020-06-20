using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly INotificationHelper notificationHelper;

        public NotificationService(AppDbContext dbContext, IMapper mapper,INotificationHelper notificationHelper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            this.notificationHelper = notificationHelper;
        }
        public async Task<IEnumerable<NotificationDto>> GetNotificationList(string userId)
        {
            return await _dbContext.Notificatons.Where(e => e.UserId == userId)
            .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task InsertNotification(NotificationDto notificationDto)
        {
            var notification = _mapper.Map<Notificaton>(notificationDto);
            await _dbContext.Notificatons.AddAsync(notification);
            await _dbContext.SaveChangesAsync();

            await notificationHelper.SendNotificationParaller(notification.UserId,"GetNotification",_mapper.Map<NotificationDto>(notification));
        }

        public async Task MarkAsReadNotification(string userId)
        {
            await _dbContext.Database.ExecuteSqlCommandAsync($@"update notificatons set isRead=1 
                                    where userid={userId} and isRead=0");
        }
    }
}