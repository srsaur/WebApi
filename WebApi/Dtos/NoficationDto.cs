using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Message { get; set; }

        public string UserId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedData { get; set; }
    }
}