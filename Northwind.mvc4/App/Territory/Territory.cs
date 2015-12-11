using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Territory
{
    public class Territory : ITerritory
    {
        public string TerritoryID { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }

    }
}