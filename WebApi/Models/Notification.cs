using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models{
    public class Notificaton{
       
        public int Id  { get; set; }
        
        [StringLength(50)]     
        public string Message { get; set; }

        public virtual AppUser User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedData { get; set; }=DateTime.Now;
    }
}