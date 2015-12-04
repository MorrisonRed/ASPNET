using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASPNET.Models
{
    public class ContactModels
    {
		[Required(ErrorMessage ="First Name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
		[Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A message must be entered")]
        public string Comment { get; set; }
    }
}

