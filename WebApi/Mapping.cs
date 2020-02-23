﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<SignUp, AppUser>().ForMember(e => e.UserName, e => e.MapFrom(x => x.Email));
                
        }
    }
}
