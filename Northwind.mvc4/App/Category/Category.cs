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
    }
}