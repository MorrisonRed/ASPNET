using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using AppCore;
using AppCore.Product;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace ASPNET.Models
{

    public class UserAddViewModel
    {
        public string SelectedRoleID { get; set; }
        public System.Web.Mvc.SelectList Roles;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //custom properties added to identiy user
        [Required]
        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public virtual string Gender { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", NullDisplayText = "")]
        public virtual DateTime? DateOfBirth { get; set; }
        [Display(Name = "Language")]
        public virtual string Language { get; set; }
        [Display(Name = "Country")]
        public virtual string Country { get; set; }
        [Display(Name = "Postal Code")]
        public virtual string PostalCode { get; set; }
    }

    public class UserEditViewModel
    {
        public string SelectedRoleID { get; set; }
        public System.Web.Mvc.SelectList Roles;

        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //custom properties added to identiy user
        [Required]
        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public virtual string Gender { get; set; }
        [DataType(DataType.DateTime)]      
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", NullDisplayText = "")]
        public virtual DateTime? DateOfBirth { get; set; }
        [Display(Name = "Language")]
        public virtual string Language { get; set; }
        [Display(Name = "Country")]
        public virtual string Country { get; set; }
        [Display(Name = "Postal Code")]
        public virtual string PostalCode { get; set; }
    }
}