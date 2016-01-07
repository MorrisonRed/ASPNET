using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Category
{
    public class Category : ICategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        #region Constructors and Destructors
        public Category()
        {

        }
        #endregion

        #region Functions and Methods
        /// <summary>
        /// Return True if all property in the object are either null or invalid
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            bool result = true;
            if (CategoryId > 0) return false;
            if (!String.IsNullOrEmpty(CategoryName)) return false;
            if (!String.IsNullOrEmpty(Description)) return false;
            if (Picture != null) return false;

            return result;
        }
        #endregion
    }
}