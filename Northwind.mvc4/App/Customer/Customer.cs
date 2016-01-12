using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Customer
{
    public class Customer : ICustomer
    {
        [Display(Name = "Customer Id"), Key]
        public string CustomerID { get; set; }
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
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
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        #region Constructors and Destructors
        public Customer()
        {

        }
        #endregion 

        #region Functions and Methods
        public bool IsEmpty()
        {
            bool result = true;
            if (!String.IsNullOrEmpty(CustomerID)) return false;
            if (!String.IsNullOrEmpty(CompanyName)) return false;
            if (!String.IsNullOrEmpty(ContactName)) return false;
            if (!String.IsNullOrEmpty(ContactTitle)) return false;
            if (!String.IsNullOrEmpty(Address)) return false;
            if (!String.IsNullOrEmpty(City)) return false;
            if (!String.IsNullOrEmpty(Region)) return false;
            if (!String.IsNullOrEmpty(PostalCode)) return false;
            if (!String.IsNullOrEmpty(Country)) return false;
            if (!String.IsNullOrEmpty(Phone)) return false;
            if (!String.IsNullOrEmpty(Fax)) return false;

            return result; 
        }
        #endregion
    }
}