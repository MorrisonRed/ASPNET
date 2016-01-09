using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using AppCore;
using AppCore.Product;

namespace ASPNET.Models
{
    public class ProductModel : IProduct
    {

        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Supplier Id")]
        public int SupplierID { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }
        [Display(Name = "Quanity Per Unit")]
        public string QuantityPerUnit { get; set; }
        [Display(Name = "Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "In Stock")]
        public int UnitsInStock { get; set; }
        [Display(Name = "On Order")]
        public int UnitsOnOrder { get; set; }
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }
    }
}