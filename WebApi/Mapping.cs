using AutoMapper;
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
            CreateMap<MessageDto, Message>().ReverseMap();
            CreateMap<Message, MessageDto>().ForMember(e => e.CreatedOn, e => e.MapFrom(x => x.MessageOn));

            CreateMap<FriendRequestInputDto, FriendRequest>();   

            CreateMap<NotificationDto,Notificaton>()
            .ForMember(e=>e.CreatedData,e=>e.Ignore());

            CreateMap<Notificaton,NotificationDto>();
        }
    }
}
