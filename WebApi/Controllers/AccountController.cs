using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
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
        public async Task<IActionResult> SignUp([FromBody] SignUp signUp)
        {
            var user = mapper.Map<AppUser>(signUp);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userManager.CreateAsync(user, signUp.Password);

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }

            if (!result.Succeeded) { return new BadRequestObjectResult(ModelState); }

            return Ok("Account Created");
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SingnIn([FromBody] LogIn logIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user =await userManager.FindByEmailAsync(logIn.Email);
            
           if(await userManager.CheckPasswordAsync(user, logIn.Password))
            {
              return Ok(await _jwtFactory.GenerateEncodedToken(user.Email));
            }
           else
            {
                ModelState.AddModelError("login_failure", "Invalid username or password.");
                return BadRequest(ModelState);
            }

        }

       


    }
}