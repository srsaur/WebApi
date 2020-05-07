using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.IServices;

namespace WebApi.Controllers
{
    [Route("api/FriendRequest")]
    public class FriendController:BaseController
    {
        private readonly IFriendRequestService requestService;

        public FriendController(IFriendRequestService requestService)
        {
            this.requestService = requestService;
        }

        [HttpGet("AcceptFriendRequest")]
        public async Task AcceptFriendRequest(string toUser)
        {
           await requestService.AcceptFriendRequest(toUser, UserId);
        }

        [HttpGet("GetFriends")]
        public async Task<dynamic> GetFriends()
        {
            return await requestService.GetFriends(UserId);
        }

        [HttpGet("GetFriendRequest")]
        public async Task<dynamic> GetFriendRequest()
        {
            return await requestService.GetFriendRequest(UserId);
        }

        [HttpPost("SendRequest")]
        public async Task SendRequest([FromBody]FriendRequestInputDto requestInputDto)
        {
            requestInputDto.RequestedFromId = UserId;
            await requestService.SendRequest(requestInputDto);
        }
    }
}
