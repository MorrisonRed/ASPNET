using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Suppliers
{
    public class Supplier : ISupplier
    {
        [Display(Name = "Id"), Key]
        public int SupplierId { get; set; }
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
        [Display(Name = "Url")]
        public string HomePage { get; set; }

        #region "Constructors and Destructors"
        public Supplier()
        {
            SetBase();
        }
        private void SetBase()
        {
            CompanyName = "";
            ContactName = "";
            Address = "";
            City = "";
            Region = "";
            PostalCode = "";
            Country = "";
            Phone = "";
            Fax = "";
            HomePage = ""; 
        }
        #endregion 

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
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            Type type = typeof(Supplier);
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                try
                {
                    info.GetValue(this, null);
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                    dt.Rows[0][info.Name] = info.GetValue(this, null);
                }
                catch
                {

                }
            }

            return dt;
        }
        #endregion
    }
}