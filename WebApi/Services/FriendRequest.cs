
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMessageService messageService;

        public FriendRequestService(AppDbContext dbContext, IMapper mapper,IMessageService messageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            this.messageService = messageService;
        }

        public async Task SendRequest(FriendRequestInputDto requestInputDto)
        {
            if (await _dbContext.FriendRequests.AnyAsync(e => (e.RequestedFromId == requestInputDto.RequestedFromId && e.RequestedToId == requestInputDto.RequestedToId) || (e.RequestedFromId == requestInputDto.RequestedToId && e.RequestedToId == requestInputDto.RequestedFromId)))
            {
                throw new System.Exception("User already Send Request");
            }
            var request = _mapper.Map<FriendRequest>(requestInputDto);
            await _dbContext.AddAsync(request);
            await _dbContext.SaveChangesAsync();

        }

        public async Task AcceptFriendRequest(string toUser, string fromUser)
        {
            var request = await _dbContext.FriendRequests.FirstOrDefaultAsync(e => e.RequestedToId == fromUser && e.RequestedFromId == toUser);
            if (request != null)
            {
                request.IsAccepted = true;
                await _dbContext.SaveChangesAsync();
                await messageService.SendMessage(new MessageDto
                {
                    FromUserId = fromUser,
                    ToUserId = toUser,
                    Text
                 = "Hello"
                });
            }
            else
            {
                throw new System.Exception("user not send any request");
            }
        }

        public async Task<dynamic> GetFriends(string userID)
        {
            return await _dbContext.FriendRequests.Where(e => (e.RequestedFromId == userID || e.RequestedToId == userID) && e.IsAccepted)
                                      .Include(e => e.RequestedFrom).Include(e => e.RequestedTo)
                 .Select(e => new
                 {
                     UserID = (e.RequestedFromId == userID) ? e.RequestedTo.Id : e.RequestedFrom.Id,
                     Name = (e.RequestedFromId == userID) ? $"{e.RequestedTo.FirstName} {e.RequestedTo.LastName}" : $"{e.RequestedFrom.FirstName} {e.RequestedFrom.LastName}",
                     Email = (e.RequestedFromId == userID) ? e.RequestedTo.UserName : e.RequestedFrom.UserName,
                     Image = (e.RequestedFromId == userID) ? e.RequestedTo.ImagePath : e.RequestedFrom.ImagePath
                 }).ToListAsync();
        }

        public async Task<dynamic> GetFriendRequest(string userId)
        {
            return await _dbContext.FriendRequests.Where(e => (e.RequestedToId == userId) && !e.IsAccepted)
                                            .Include(e => e.RequestedFrom)
                                            .Select(e => new
                                            {
                                                UserID = e.RequestedFrom.Id,
                                                Name = $"{e.RequestedFrom.FirstName} {e.RequestedFrom.LastName}",
                                                Email = e.RequestedFrom.UserName,
                                                Image = e.RequestedFrom.ImagePath,
                                                e.IsAccepted
                                            }).ToListAsync(); ;
        }

        public async Task<IList<string>> GetFriendIds(string userId)
        {
            return await _dbContext.FriendRequests.Where(e => (e.RequestedFromId == userId || e.RequestedToId == userId) && e.IsAccepted)
                                     .Include(e => e.RequestedFrom).Include(e => e.RequestedTo).Select(e => (e.RequestedFromId == userId) ? e.RequestedTo.Id : e.RequestedFrom.Id).ToListAsync();
        }
    }
}