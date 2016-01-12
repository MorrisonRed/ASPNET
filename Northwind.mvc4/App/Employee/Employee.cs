using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Employee
{
    public class Employee : IEmployee
    {
        [Display(Name = "Employee Id"), Key]
        public int EmployeeID { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Title of Courtesy")]
        public string TitleOfCourtesy { get; set; }
        [Display(Name = "BirthDate")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "HireDate")]
        public DateTime? HireDate { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Extension")]
        public string Extension { get; set; }
        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "ReportsTo")]
        public int? ReportsTo { get; set; }
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }
        [Display(Name = "Salary")]
        public decimal? Salary { get; set; }

        #region Constructors and Destructors
        public Employee()
        {

        }
        #endregion

        #region Functions and Methods
        public bool IsEmpty()
        {
            bool result = true;
            if (EmployeeID > 0) return false;
            if (!String.IsNullOrEmpty(LastName)) return false;
            if (!String.IsNullOrEmpty(FirstName)) return false;
            if (!String.IsNullOrEmpty(Title)) return false;
            if (!String.IsNullOrEmpty(TitleOfCourtesy)) return false;
            if (BirthDate.HasValue) return false;
            if (HireDate.HasValue) return false;
            if (!String.IsNullOrEmpty(Address)) return false;
            if (!String.IsNullOrEmpty(City)) return false;
            if (!String.IsNullOrEmpty(Region)) return false;
            if (!String.IsNullOrEmpty(PostalCode)) return false;
            if (!String.IsNullOrEmpty(Country)) return false;
            if (!String.IsNullOrEmpty(HomePhone)) return false;
            if (!String.IsNullOrEmpty(Extension)) return false;
            if (Photo != null) return false;
            if (!String.IsNullOrEmpty(Notes)) return false;
            if (!String.IsNullOrEmpty(PhotoPath)) return false;
            if (Salary.HasValue) return false;

            return result;
        }
        #endregion
    }
}