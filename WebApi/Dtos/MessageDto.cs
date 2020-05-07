using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class MessageDto
    {
        public long Id { get; set; }
        
        [Required]
        public string Text { get; set; }

      
        public string FromUserId { get; set; }

        [Required]
        public string ToUserId { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
