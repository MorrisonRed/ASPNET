using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.City
{
    public interface ICity
    {
        int ID { get; set; }
        string Name { get; set; }
        string CountryCode { get; set; }
        string District { get; set; }
        int Population { get; set; }
    }
}
