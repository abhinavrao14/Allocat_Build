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
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
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