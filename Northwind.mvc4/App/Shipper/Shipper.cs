using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Shipper
{
    public class Shipper : IShipper
    {
        [Display(Name = "Id"), Key]
        public int ShipperID { get; set; }
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        #region "Constructors and Destructors"
        public Shipper()
        {
            SetBase();
        }
        private void SetBase()
        {
            CompanyName = "";
            Phone = "";
        }
        #endregion

        #region Functions and Methods
        public bool IsEmpty()
        {
            bool result = true;
            if (ShipperID > 0) return false;
            if (!String.IsNullOrEmpty(CompanyName)) return false;
            if (!String.IsNullOrEmpty(Phone)) return false;

            return result;
        }
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            Type type = typeof(Shipper);
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                try
                {
                    info.GetValue(this, null);
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                    dt.Rows[0][info.Name] = info.GetValue(this, null);
                }
                catch
                {

                }
            }

            return dt;
        }
        #endregion
    }
}
