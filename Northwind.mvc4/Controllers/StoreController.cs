using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ASPNET.Models;
using System.Text;
using System.Configuration;
using AppCore.Product;

namespace ASPNET.Controllers
{
    public class StoreController : Controller
    {
        private ProductModel product = new ProductModel();
        private ProductRepository<ProductModel> _productRepository = 
            new ProductRepository<ProductModel>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

        public StoreController(IProduct product)
        {
            this.product = (ProductModel)product;
        }

        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products(string searchterm)
        {
            IQueryable<ProductModel> products;
            if (string.IsNullOrEmpty(searchterm))
            {
                products = _productRepository.GetProducts();
            }
            else
            {
                products = _productRepository.GetProducts().Where(p => p.ProductName.ToLower().Contains(searchterm.ToLower()));
            }
            
            return View(products);
        }
    }
}