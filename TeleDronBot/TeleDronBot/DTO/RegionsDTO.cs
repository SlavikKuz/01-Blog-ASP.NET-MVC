using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeleDronBot.DTO
{
    class RegionsDTO
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
