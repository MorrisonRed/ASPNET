using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Country
{
    public class Country : ICountry
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public string Region { get; set; }
        public float SurfaceArea { get; set; }
        public int IndependenceYear { get; set; }
        public int Population { get; set; }
        public float LifeExpectancy { get; set; }
        public float GNP { get; set; }
        public float GNPOld { get; set; }
        public string LocalName { get; set; }
        public string GovernmentForm { get; set; }
        public string HeadOfState { get; set; }
        public string Code2 { get; set; }
    }
}
