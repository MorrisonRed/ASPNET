using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Region
{
    public class Region : IRegion
    {
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

    }
}