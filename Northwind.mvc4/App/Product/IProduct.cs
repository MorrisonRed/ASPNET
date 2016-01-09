using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Product
{
    public interface IProduct
    {
        int ProductID { get; set; }
        string ProductName { get; set; }
        int SupplierID { get; set; }
        int CategoryID { get; set; }
        string QuantityPerUnit { get; set; }
        decimal UnitPrice { get; set; }
        int UnitsInStock { get; set; }
        int UnitsOnOrder { get; set; }
        int ReorderLevel { get; set; }
        bool Discontinued { get; set; }

        bool IsEmpty();
    }
}
