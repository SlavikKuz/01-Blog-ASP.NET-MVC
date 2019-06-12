using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPBlog2.Models
{
    public class UsersModel
    {
        public int age { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string confirmEmail { get; set; }
        public string passport { get; set; }
        public string confirmPassport { get; set; }

    }
}