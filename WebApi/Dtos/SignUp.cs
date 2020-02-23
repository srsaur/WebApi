using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class SignUp
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Range(18,60,ErrorMessage ="Age between 18 to 60")]
        public int Age { get; set; }
        public string Password { get; set; }
    }
}
