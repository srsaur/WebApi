using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/user")]
   // [Authorize]
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext dbContext;

        public UserController(UserManager<AppUser> userManager,AppDbContext dbContext)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
        }

        [HttpGet("getAppUsers")]
        public async Task<IEnumerable<dynamic>> GetAppUsers()
        {
            return await _userManager.Users.Where(e=>e.Id!= UserId).Select(e =>
                        new {
                            e.Id,
                            e.FirstName,
                            e.LastName,
                            e.Email,
                            e.Address,
                            e.Gender,
                            e.DOB,
                            isSend= dbContext.FriendRequests.Any(x=>x.RequestedFromId==UserId && x.RequestedToId==e.Id || x.RequestedFromId == e.Id && x.RequestedToId == UserId),
                            e.ImagePath
                        }).ToListAsync();
        }
    }
}