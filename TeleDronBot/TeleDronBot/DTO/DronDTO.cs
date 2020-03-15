using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeleDronBot.DTO
{
    class DronDTO
    {
        [Key]
        public int DronID { get; set; }
        public string Mark { get; set; }
    }
}
