using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace AppCore.City
{
    public class City : ICity
    {
        [Display(Name="Id")]
        public int ID { get; set; }
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name="Country Code")]
        public string CountryCode { get; set; }
        [Display(Name = "District")]
        public string District { get; set; }
        [Display(Name = "Population")]
        public int Population { get; set; }
    }
}