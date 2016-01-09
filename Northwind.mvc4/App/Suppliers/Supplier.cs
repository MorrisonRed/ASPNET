using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Suppliers
{
    public class Supplier : ISupplier
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }

        #region Functions Methods
        public bool IsEmpty()
        {
            bool result = true;
            if (SupplierId > 0) return false;
            if (!String.IsNullOrEmpty(CompanyName)) return false;
            if (!String.IsNullOrEmpty(ContactName)) return false;
            if (!String.IsNullOrEmpty(Address)) return false;
            if (!String.IsNullOrEmpty(City)) return false;
            if (!String.IsNullOrEmpty(Region)) return false;
            if (!String.IsNullOrEmpty(PostalCode)) return false;
            if (!String.IsNullOrEmpty(Country)) return false;
            if (!String.IsNullOrEmpty(Phone)) return false;
            if (!String.IsNullOrEmpty(Fax)) return false;
            if (!String.IsNullOrEmpty(HomePage)) return false;

            return result; 
        }
        #endregion
    }
}