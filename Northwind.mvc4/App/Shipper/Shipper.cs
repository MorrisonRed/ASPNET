using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Shipper
{
    public class Shipper : IShipper
    {
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
