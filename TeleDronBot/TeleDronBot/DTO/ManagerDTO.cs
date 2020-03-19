using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeleDronBot.DTO
{
    class ManagerDTO
    {
        [Key]
        public int Id { get; set; }

        public long ChatId { get; set; }
    }
}
