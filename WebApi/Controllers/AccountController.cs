using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularASPNETCore2WebApiAuth.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IJwtFactory _jwtFactory;

        public AccountController(IMapper mapper, UserManager<AppUser> userManager, IJwtFactory jwtFactory)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this._jwtFactory = jwtFactory;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]SignUp signUp)
        {
            var user = mapper.Map<AppUser>(signUp);
            var result = await userManager.CreateAsync(user, signUp.Password);

            StringBuilder sb = new StringBuilder();
            foreach (var item in result.Errors)
            {
                sb.AppendLine($"{item.Code}: {item.Description}\n");
            }

            if (!result.Succeeded) { throw new Exception(sb.ToString()); }

            return Ok("Account Created");
        }

        [HttpPost("SignIn")]
        public async Task<string> SingnIn([FromBody] LogIn logIn)
        {    
           var user =await userManager.FindByEmailAsync(logIn.Email);
           if(await userManager.CheckPasswordAsync(user, logIn.Password))
            {
              return  await _jwtFactory.GenerateEncodedToken(user);
            }
           else
            {
                throw new Exception("Invalid UserName");
            }
        }

    }
}