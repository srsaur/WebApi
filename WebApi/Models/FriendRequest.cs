using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FriendRequest
    {
        public long Id { get; set; }

        public virtual AppUser RequestedFrom { get; set; }

        [ForeignKey("RequestedFrom")]
        public string RequestedFromId { get; set; }

        public virtual AppUser RequestedTo { get; set; }

        [ForeignKey("RequestedTo")]
        public string RequestedToId { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
