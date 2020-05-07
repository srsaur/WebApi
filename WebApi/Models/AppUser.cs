using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }

        [PersonalData]
        [Column(TypeName ="Date")]
        public DateTime DOB { get; set; }

        public string ImagePath { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Others
    }
}
