using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class FriendRequestInputDto
    {
        public string RequestedFromId { get; set; }

        public string RequestedToId { get; set; }
    }
}
