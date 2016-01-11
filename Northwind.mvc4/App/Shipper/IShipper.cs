using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Shipper
{
    public interface IShipper
    {
        int ShipperID { get; set;  }
        string CompanyName { get; set; }
        string Phone { get; set; }

        bool IsEmpty();
    }
}
