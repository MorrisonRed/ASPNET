using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Region
{
    public class Region : IRegion
    {
        [Display(Name = "Id"), Key]
        public int RegionID { get; set; }
        [Display(Name = "Description")]
        public string RegionDescription { get; set; }


        #region Functions and SubRoutines
        public bool IsEmpty()
        {
            bool result = true;
            if (RegionID > 0) return false;
            if (!string.IsNullOrEmpty(RegionDescription)) return false;

            return result;
        }
        #endregion
    }
}