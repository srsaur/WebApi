using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.IServices
{
   public interface IFriendRequestService
    {
        Task SendRequest(FriendRequestInputDto requestInputDto);
        Task AcceptFriendRequest(string toUser, string fromUser);
        Task<dynamic> GetFriends(string userID);
        Task<dynamic> GetFriendRequest(string userId);
        Task<IList<string>> GetFriendIds(string userId);
    }
}
