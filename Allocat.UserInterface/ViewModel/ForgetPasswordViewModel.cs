using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Allocat.Web.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [Display(Name = "Email-Id")]
        public string EmailId { get; set; }
    }
}