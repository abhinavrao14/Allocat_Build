using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Allocat.Web.Models
{
    public class SignUpViewModel
    {
        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets with spaces allowed")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only alphabets and numbers are allowed")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "EmailId")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Security Question")]
        public string SecurityQuestion { get; set; }

        [Required]
        [Display(Name = "Security Answer")]
        public string SecurityAnswer { get; set; }
    }
}