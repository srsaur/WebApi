using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [CheckModel]
    public class BaseController : Controller
    {
        public string UserName
        {
            get
            {
                return User.Claims.FirstOrDefault(e => e.Type == "EmailId").Value;
            }
        }

        public string UserId
        {
            get
            {
                return User.Claims.FirstOrDefault(e => e.Type == "sid").Value;
            }
        }
    }
}