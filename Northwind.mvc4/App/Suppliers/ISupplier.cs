using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Suppliers
{
    public interface ISupplier
    {
        int SupplierId { get; set; }
        string CompanyName { get; set; }
        string ContactName { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string Region { get; set; }
        string PostalCode { get; set; }
        string Country { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        string HomePage { get; set; }

        bool IsEmpty();
    }
}
