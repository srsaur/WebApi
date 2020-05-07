using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RelationKeys
    {
        public long Id { get; set; }

        public virtual AppUser FromUser { get; set; }

        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        public virtual AppUser ToUser { get; set; }

        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        public string Key { get {
                return $"{FromUserId}{ToUserId}";
            } }
    }
}
