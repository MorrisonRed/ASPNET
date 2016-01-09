using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace AppCore.Product
{
    public class Product : IProduct
    {
        [Display(Name = "Id")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Supplier Id")]
        public int SupplierID { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }
        [Display(Name = "Quanitity per Unit")]
        public string QuantityPerUnit { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Units in Stock")]
        public int UnitsInStock { get; set; }
        [Display(Name = "Units on Order")]
        public int UnitsOnOrder { get; set; }
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        #region Functions and Methods
        public bool IsEmpty()
        {
            bool result = true;
            if (ProductID > 0) return false;
            if (SupplierID > 0) return false;
            if (CategoryID > 0) return false;
            if (!String.IsNullOrEmpty(QuantityPerUnit)) return false;
            if (UnitPrice > 0) return false;
            if (UnitsInStock > 0) return false;
            if (UnitsOnOrder > 0) return false;
            if (ReorderLevel > 0) return false;

            return result;
        }
        #endregion
    }
}