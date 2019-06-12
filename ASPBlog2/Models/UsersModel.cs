using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPBlog2.Models
{
    public class UsersModel
    {
        [Display(Name="Age")]
        [Range(7,70, ErrorMessage ="Please, enter the age from 7 to 70 years.")]
        public int age { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage ="Please fill in the field Name.")]
        public string name { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please fill in the field Last Name.")]
        public string lastname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please fill in the field Email.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The Email must be from 4 to 50 symbols.")]
        public string email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm Email")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The Email must be from 4 to 50 symbols.")]
        [Required(ErrorMessage = "Please fill in the field Confirm Email.")]
        [Compare("email", ErrorMessage ="Email confirm is wrong!")]
        public string confirmEmail { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please fill in the field Password.")]
        [StringLength(50, MinimumLength =4, ErrorMessage ="The password must be from 4 to 50 symbols.")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password confirm is wrong!")]
        public string confirmPassword { get; set; }
    }
}