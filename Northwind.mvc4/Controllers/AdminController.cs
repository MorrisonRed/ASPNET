using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ASPNET.Models;
using System.Collections.Generic;

using System.Configuration;
using AppCore;

namespace ASPNET.Models
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        #region Public Properties
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region Constructors and Destructors
        public AdminController()
        {
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Roles
        public ActionResult Roles(string searchterm)
        {
            //var context = RoleManager;
            var context = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            List<ApplicationRole> result = new List<ApplicationRole>();

            if (string.IsNullOrEmpty(searchterm))
            {
                var roles = context.Roles;
                foreach(var r in roles)
                {
                    result.Add((ApplicationRole)r);
                }
                return View(result);
            }
            else
            {
                var roles = context.Roles.Where(r => r.Name.ToLower().Contains(searchterm.Trim().ToLower())).OrderBy(r => r.Name);
                foreach (var r in roles)
                {
                    result.Add((ApplicationRole)r);
                }
                return View(result);
            }           
        }

        public ActionResult RoleAdd()
        {
            ViewBag.PageTitle = "Add Role";
            ViewBag.Message = "";
            

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleAdd(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    IsSytemAccount = model.IsSytemAccount
                };

                var result = await RoleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Role added!";
                    View();
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult RoleEdit(string id)
        {
            ViewBag.PageTitle = "Edit Role";
            ViewBag.Message = "";

            var role = RoleManager.FindById(id);

            return View((ApplicationRole)role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleEdit(ApplicationRole model)
        {
            ViewBag.PageTitle = "Edit Role";
            ViewBag.Message = "";

            if (ModelState.IsValid)
            {
                var role = new ApplicationRole()
                {
                    Id = model.Id,
                    Name = model.Name,
                    IsSytemAccount = model.IsSytemAccount
                };

                var result = await RoleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Role updated!";
                    View();
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult RoleDelete(string id = "")
        {
            ViewBag.PageTitle = "Delete Role";
            ViewBag.Message = "";

            var role = RoleManager.FindById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost, ActionName("RoleDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleDeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    ViewBag.Message = "Role deleted!";
                    ViewBag.Message = result.Errors.ToString();
                    return View(role);
                }

                return RedirectToAction("Roles", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Users
        public ActionResult Users(string searchterm)
        {
            var context = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ApplicationUser> result = new List<ApplicationUser>();

            if (string.IsNullOrEmpty(searchterm))
            {
                var users = context.Users;
                foreach (var u in users)
                {
                    result.Add((ApplicationUser)u);
                }
                return View(result);
            }
            else
            {
                var users = context.Users.Where(user => user.UserName.ToLower().Contains(searchterm.Trim().ToLower())).OrderBy(user => user.LastName);
                foreach (var u in users)
                {
                    result.Add((ApplicationUser)u);
                }
                return View(result);
            }
        }

        public ActionResult UserAdd(UserAddViewModel model)
        {
            ViewBag.PageTitle = "New User";
            ViewBag.Message = "";

            //create dropdowns
            model.Roles = new SelectList(RoleManager.Roles, "Name", "Name");
            model.SelectedRoleID = "User";
            ViewBag.Languages = SelectListForLanguages("en");
            ViewBag.Countries = SelectListForCountries("CAN");
            ViewBag.Genders = SelectListForGender("F");

            //ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserAdd(UserAddViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = form["Gender"].ToString(),
                    DateOfBirth = model.DateOfBirth,
                    Language = form["Lanaguage"].ToString(),
                    Country = form["Country"].ToString(),
                    PostalCode = model.PostalCode
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //get selected role from dropdown
                    var rolename = model.SelectedRoleID;
                    var roleresult = UserManager.AddToRole(user.Id, rolename);

                    ModelState.Clear();
                    ViewBag.Message = "User added!";
                    View();
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            //create dropdowns
            var roles = RoleManager.Roles;
            model.Roles = new SelectList(roles, "Name", "Name");
            ViewBag.Languages = SelectListForLanguages("en");
            ViewBag.Countries = SelectListForCountries("CAN");
            ViewBag.Genders = SelectListForGender("Female");

            return View(model);
        }

        public ActionResult UserEdit(string id)
        {
            ViewBag.PageTitle = "Edit User";
            ViewBag.Message = "";

            //load user
            var user = UserManager.FindById(id);

            //create dropdowns and set default values
            var userrole = RoleManager.FindById(user.Roles.First().RoleId);
            var roles = RoleManager.Roles;        
            
                
            //ViewBag.Roles = new SelectList(roles, "Name", "Name", userrole.Name);
            ViewBag.Languages = SelectListForLanguages(user.Language);
            ViewBag.Countries = SelectListForCountries(user.Country);
            ViewBag.Genders = SelectListForGender(user.Gender);

            UserEditViewModel model = new UserEditViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Language = user.Language,
                Country = user.Country,
                PostalCode = user.PostalCode,

                SelectedRoleID = userrole.Name,
                Roles = new SelectList(roles, "Name", "Name", userrole.Name)
        };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(UserEditViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //load the user and update the values
                var user = UserManager.FindById(model.Id);
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = form["Gender"].ToString();
                user.DateOfBirth = model.DateOfBirth;
                user.Language = form["Lanaguage"].ToString();
                user.Country = form["Country"].ToString();
                user.PostalCode = model.PostalCode;

                //load current role for user
                var userrole = RoleManager.FindById(user.Roles.First().RoleId);

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //get selected role from dropdown if different than
                    //current role then update
                    //if(userrole.Name.ToLower() != form["Roles"].ToString().ToLower())
                    if (userrole.Name.ToLower() != model.SelectedRoleID.ToLower())
                    {
                        var roleresult = UserManager.RemoveFromRole(user.Id, userrole.Name);
                        if(roleresult.Succeeded)
                        {
                            //UserManager.AddToRole(user.Id, form["Roles"].ToString());
                            UserManager.AddToRole(user.Id, model.SelectedRoleID.ToString());
                        }
                        else
                        {
                            AddErrors(roleresult);
                        }
                    }
                    ViewBag.Message = "User updated!";
                    return View();
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            //create dropdowns
            var roles = RoleManager.Roles;
            model.Roles = new SelectList(roles, "Name", "Name");
            ViewBag.Languages = SelectListForLanguages("en");
            ViewBag.Countries = SelectListForCountries("CAN");
            ViewBag.Genders = SelectListForGender("M");

            return View(model);
        }

        public ActionResult UserDelete(string id = "")
        {
            ViewBag.PageTitle = "Delete User";
            ViewBag.Message = "";

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDeleteConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    ViewBag.Message = "User deleted!";
                    ViewBag.Message = result.Errors.ToString();
                    return View(user);
                }

                return RedirectToAction("Users", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Categories
        public ActionResult Categories(string searchterm)
        {
            IQueryable<AppCore.Category.Category> categories;
            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (string.IsNullOrEmpty(searchterm))
            {
                categories = categoryRepository.GetCategories().OrderBy(c => c.CategoryName);
            }
            else
            {
                categories = categoryRepository.GetCategories().Where(c => c.CategoryName.ToLower().Contains(searchterm.ToLower())).OrderBy(c => c.CategoryName);
            }

            return View(categories);
        }

        public ActionResult CategoryAdd()
        {
            ViewBag.Title = "Add Category";
            ViewBag.Message = "";

            return View();

        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CategoryAdd(CategoryAddViewModel model, FormCollection form, HttpPostedFileBase NewPicture)
        {
            if (ModelState.IsValid)
            {
                var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                AppCore.Category.Category cat = new AppCore.Category.Category();
                cat.CategoryName = model.CategoryName;
                cat.Description = model.Description;
                if (NewPicture != null && NewPicture.ContentLength > 0)
                {
                    string filetype = NewPicture.ContentType;
                    Int32 length = NewPicture.ContentLength;
                    byte[] tempImage = new byte[length];
                    NewPicture.InputStream.Read(tempImage, 0, length);
                    cat.Picture = tempImage;
                    model.Picture = tempImage;
                }

                var result = categoryRepository.Add(cat);
                if (result)
                {
                    ViewBag.Message = "Category added!";
                    model = new CategoryAddViewModel();
                    ModelState.Clear();
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to add category"));
                //AddErrors(result);
            }

            return View(model);
        }
       
        public ActionResult CategoryEdit(int id)
        {
            ViewBag.PageTitle = "Edit Category";
            ViewBag.Message = "";

            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var category = categoryRepository.FindById(id);
            if (category == null)
            {
                //category did not load go to error page
                return RedirectToAction("Error", "Home");
            }

            CategoryEditViewModel model = new CategoryEditViewModel() {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Picture = category.Picture
            };

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CategoryEdit(CategoryEditViewModel model, FormCollection form, HttpPostedFileBase NewPicture)
        {
            if (ModelState.IsValid)
            {
                //load category and update values
                var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                var cat = categoryRepository.FindById(model.CategoryId);

                //get picture
                if (NewPicture != null && NewPicture.ContentLength > 0)
                {
                    string filetype = NewPicture.ContentType;
                    Int32 length = NewPicture.ContentLength;
                    byte[] tempImage = new byte[length];
                    NewPicture.InputStream.Read(tempImage, 0, length);
                    cat.Picture = tempImage;
                    model.Picture = tempImage;
                }
                
                cat.CategoryName = model.CategoryName;
                cat.Description = model.Description;
 
                //update category        
                var result = categoryRepository.Update(cat);
                if(result)
                {

                    ViewBag.Message = "Category updated!";
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to update category"));
                //AddErrors(result);
            }
            return View(model);
        }

        public ActionResult CategoryDelete(int id = 0)
        {
            ViewBag.PageTitle = "Delete Category";
            ViewBag.Message = "";

            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cat = categoryRepository.FindById(id);
            if (cat.IsEmpty())
            {
                return HttpNotFound();
            }
            var model = new CategoryDeleteViewModel()
            {
                CategoryId = cat.CategoryId,
                CategoryName = cat.CategoryName,
                Description = cat.Description,
                Picture = cat.Picture
            };
            return View(model);
        }
        [HttpPost, ActionName("CategoryDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CategoryDeleteConfirmed(int id)
        {
            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cat = categoryRepository.FindById(id);

            if (!cat.IsEmpty())
            {
                var result = categoryRepository.Delete(cat);   

                if (result)
                {
                    ViewBag.Message = "Category deleted!";
                    var model = new CategoryDeleteViewModel();                 
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Categories", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Products
        public ActionResult Products(string searchterm)
        {
            IQueryable<ASPNET.Models.ProductViewModel> products;
            var productRepository = new AppCore.Product.ProductRepository<ProductViewModel>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (string.IsNullOrEmpty(searchterm))
            {
                products = productRepository.GetProducts().OrderBy(p => p.ProductName);
            }
            else
            {
                products = productRepository.GetProducts().Where(p => p.ProductName.ToLower().Contains(searchterm.ToLower())).OrderBy(p => p.ProductName);
            }

            return View(products);
        }

        public ActionResult ProductAdd(ProductAddViewModel model)
        {
            ViewBag.PageTitle = "New Product";
            ViewBag.Message = "";

            //create dropdowns
            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            model.Categories = new SelectList(categoryRepository.GetCategories(), "CategoryID", "CategoryName");
            model.SelectedCategoryID = 1;
            model.Suppliers = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
            model.SelectedSupplierID = 1;


            //ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductAdd(ProductAddViewModel model, FormCollection form)
        {
            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                var productRepository = new AppCore.Product.ProductRepository<AppCore.Product.Product>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                AppCore.Product.Product prod = new AppCore.Product.Product();
                //prod.ProductID = model.ProductID;
                prod.ProductName = model.ProductName;
                prod.SupplierID = model.SelectedSupplierID;
                prod.CategoryID = model.SelectedCategoryID;
                prod.QuantityPerUnit = model.QuantityPerUnit;
                prod.UnitPrice = model.UnitPrice;
                prod.UnitsInStock = model.UnitsInStock;
                prod.UnitsOnOrder = model.UnitsOnOrder;
                prod.ReorderLevel = model.ReorderLevel;
                prod.Discontinued = model.Discontinued;

                var result = productRepository.Add(prod);
                if (result)
                {
                    ViewBag.Message = "product added!";
                    model = new ProductAddViewModel();
                    ModelState.Clear();

                    model.Categories = new SelectList(categoryRepository.GetCategories(), "CategoryID", "CategoryName");
                    model.SelectedCategoryID = 1;
                    model.Suppliers = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
                    model.SelectedSupplierID = 1;
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to add product"));
                //AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            //create dropdowns
            //create dropdowns
            model.Categories = new SelectList(categoryRepository.GetCategories(), "CategoryID", "CategoryName");
            model.SelectedCategoryID = 1;
            model.Suppliers = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
            model.SelectedSupplierID = 1;

            return View(model);
        }

        public ActionResult ProductEdit(int id)
        {
            ViewBag.PageTitle = "Edit Product";
            ViewBag.Message = "";
       
            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var productRepository = new AppCore.Product.ProductRepository<AppCore.Product.Product>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var prod = productRepository.FindById(id);
            if (prod.IsEmpty())
            {
                //category did not load go to error page
                return RedirectToAction("Error", "Home");
            }

            ProductEditViewModel model = new ProductEditViewModel()
            {
                ProductID = prod.ProductID,
                ProductName = prod.ProductName,
                SupplierID = prod.SupplierID,
                CategoryID = prod.CategoryID,
                QuantityPerUnit = prod.QuantityPerUnit,
                UnitPrice = prod.UnitPrice,
                UnitsInStock = prod.UnitsInStock,
                UnitsOnOrder = prod.UnitsOnOrder,
                ReorderLevel = prod.ReorderLevel,
                Discontinued = prod.Discontinued,

                SelectedCategoryID = prod.CategoryID,
                Categories = new SelectList(categoryRepository.GetCategories(), "CategoryID", "CategoryName", prod.CategoryID),
                SelectedSupplierID = prod.SupplierID,
                Suppliers = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName", prod.SupplierID)
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductEdit(ProductEditViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                var productRepository = new AppCore.Product.ProductRepository<AppCore.Product.Product>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                var prod = productRepository.FindById(model.ProductID);

                prod.ProductID = model.ProductID;
                prod.ProductName = model.ProductName;
                prod.SupplierID = model.SelectedSupplierID;
                prod.CategoryID = model.SelectedCategoryID;
                prod.QuantityPerUnit = model.QuantityPerUnit;
                prod.UnitPrice = model.UnitPrice;
                prod.UnitsInStock = model.UnitsInStock;
                prod.UnitsOnOrder = model.UnitsOnOrder;
                prod.ReorderLevel = model.ReorderLevel;
                prod.Discontinued = model.Discontinued;

                //update category        
                var result = productRepository.Update(prod);
                if (result)
                {
                    ViewBag.Message = "Product updated!";
                    model.Categories = new SelectList(categoryRepository.GetCategories(), "CategoryID", "CategoryName");
                    model.SelectedCategoryID = prod.CategoryID;
                    model.Suppliers = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
                    model.SelectedSupplierID = prod.SupplierID;
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to update product"));
                //AddErrors(result);
            }
            return View(model);
        }

        public ActionResult ProductDelete(int id = 0)
        {
            ViewBag.PageTitle = "Delete Product";
            ViewBag.Message = "";

            var categoryRepository = new AppCore.Category.CategoryRepository<AppCore.Category.Category>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var productRepository = new AppCore.Product.ProductRepository<AppCore.Product.Product>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var prod = productRepository.FindById(id);
            if (prod.IsEmpty())
            {
                return HttpNotFound();
            }

            var cat = categoryRepository.FindById(prod.CategoryID);
            var sup = supplierRepository.FindById(prod.SupplierID);



            var model = new ProductDeleteViewModel()
            {
                ProductID = prod.ProductID,
                ProductName = prod.ProductName,
                SupplierID = prod.SupplierID,
                CategoryID = prod.CategoryID,
                QuantityPerUnit = prod.QuantityPerUnit,
                UnitPrice = prod.UnitPrice,
                UnitsInStock = prod.UnitsInStock,
                UnitsOnOrder = prod.UnitsOnOrder,
                ReorderLevel = prod.ReorderLevel,
                Discontinued = prod.Discontinued,
            };
            if (!sup.IsEmpty()) model.Supplier = sup.CompanyName;
            if (!cat.IsEmpty()) model.Category = cat.CategoryName;

            return View(model);
        }
        [HttpPost, ActionName("ProductDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductDeleteConfirmed(int id)
        {
            var productRepository = new AppCore.Product.ProductRepository<AppCore.Product.Product>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var prod = productRepository.FindById(id);

            if (!prod.IsEmpty())
            {
                var result = productRepository.Delete(prod);

                if (result)
                {
                    ViewBag.Message = "product deleted!";
                    var model = new ProductDeleteViewModel();
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Products", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Suppliers
        public ActionResult Suppliers(string searchterm)
        {
            ViewBag.PageTitle = "Suppliers";
            ViewBag.Message = "";

            IQueryable<AppCore.Suppliers.Supplier> suppliers;
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (!String.IsNullOrEmpty(searchterm))
            {
                suppliers = supplierRepository.GetSuppliers().Where(s => s.CompanyName.ToLower().Contains(searchterm.ToLower())).OrderBy(s => s.CompanyName);
            }
            else
            {
                suppliers = supplierRepository.GetSuppliers().OrderBy(s => s.CompanyName);
            }

            return View(suppliers);
        }

        public ActionResult SupplierAdd(SupplierAddViewModel model)
        {
            ViewBag.PageTitle = "Add Supplier";
            ViewBag.Message = "";

            //load dropdownlists
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SupplierAdd(SupplierAddViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                var supplier = new AppCore.Suppliers.Supplier()
                {
                    CompanyName = model.CompanyName,
                    ContactName = model.ContactName,
                    ContactTitle = model.ContactTitle,
                    Address = model.Address, 
                    City = model.City, 
                    Region = model.Region, 
                    PostalCode = model.PostalCode, 
                    Country = model.Country, 
                    Phone = model.Phone, 
                    Fax = model.Fax, 
                    HomePage = model.HomePage
                };

                var result = supplierRepository.Add(supplier); 
                if (result)
                {
                    ViewBag.Message = "supplier added"; 
                    model = new SupplierAddViewModel();
                    ModelState.Clear();
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", new Exception("failed to add supplier"));
                    //AddErrors(result);
                }
            }

            //not valid
            return View(model);
        }

        public ActionResult SupplierEdit(int id)
        {
            ViewBag.PageTitle = "Edit Supplier";
            ViewBag.Message = "";

            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplier = supplierRepository.FindById(id);

            if (supplier.IsEmpty())
            {
                return RedirectToAction("Error", "Home");
            }

            SupplierEditViewModel model = new SupplierEditViewModel()
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle,
                Address = supplier.Address,
                City = supplier.City,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                HomePage = supplier.HomePage
            };

            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
            model.SelectedCountryID = model.Country;
            

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SupplierEdit(SupplierEditViewModel model, FormCollection form)
        {
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                var supplier = supplierRepository.FindById(model.SupplierId);
                supplier.CompanyName = model.CompanyName;
                supplier.ContactName = model.ContactName;
                supplier.ContactTitle = model.ContactTitle;
                supplier.Address = model.Address;
                supplier.City = model.City;
                supplier.Region = model.Region;
                supplier.PostalCode = model.PostalCode;
                supplier.Country = model.SelectedCountryID;
                supplier.Phone = model.Phone;
                supplier.Fax = model.Fax;
                supplier.HomePage = model.HomePage;

                //update supplier
                var result = supplierRepository.Update(supplier);
                if (result)
                {
                    ViewBag.Message = "supplier updated!";
                    model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
                    model.SelectedCountryID = supplier.Country;
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to update supplier"));
                //AddErrors(result);
            }

            //only runs is model state is invalid
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
            return View(model);
        }

        public ActionResult SupplierDelete(int id = 0)
        {
            ViewBag.PageTitle = "Delete Supplier";
            ViewBag.Message = "";

            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplier = supplierRepository.FindById(id);

            if (supplier.IsEmpty())
            {
                return RedirectToAction("Error", "Home");
            }

            SupplierDeleteViewModel model = new SupplierDeleteViewModel()
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle,
                Address = supplier.Address,
                City = supplier.City,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                HomePage = supplier.HomePage
            };

            return View(model);

        }
        [HttpPost, ActionName("SupplierDelete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> SupplierDeleteConfirmed(int id)
        {
            var supplierRepository = new AppCore.Suppliers.SupplierRepository<AppCore.Suppliers.Supplier>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var supplier = supplierRepository.FindById(id);

            if (!supplier.IsEmpty())
            { 
                var result = supplierRepository.Delete(supplier);

                if (result)
                {
                    ViewBag.Message = "supplier deleted!";
                    var model = new SupplierDeleteViewModel();
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Suppliers", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Shippers
        public ActionResult Shippers(string searchterm)
        {
            ViewBag.PageTitle = "Shippers";
            ViewBag.Message = "";

            IQueryable<AppCore.Shipper.Shipper> shippers;
            var shipperRepository = new AppCore.Shipper.ShipperRepository<AppCore.Shipper.Shipper>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());     

            if (!String.IsNullOrEmpty(searchterm))
            {
                shippers = shipperRepository.GetShippers().Where(s => s.CompanyName.ToLower().Contains(searchterm.ToLower())).OrderBy(s => s.CompanyName);
            }
            else
            {
                shippers = shipperRepository.GetShippers().OrderBy(s => s.CompanyName);
            }
            return View(shippers);
        }

        public ActionResult ShipperAdd(ShipperAddViewModel model)
        {
            ViewBag.PageTitle = "Add Shipper";
            ViewBag.Message = "";

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ShipperAdd(ShipperAddViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var shipperRepository = new AppCore.Shipper.ShipperRepository<AppCore.Shipper.Shipper>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                AppCore.Shipper.Shipper shipper = new AppCore.Shipper.Shipper()
                {
                    CompanyName = model.CompanyName,
                    Phone = model.Phone
                };

                var result = shipperRepository.Add(shipper);
                if (result)
                {
                    ViewBag.Message = "shipper added";
                    model = new ShipperAddViewModel();
                    ModelState.Clear();
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", new Exception("failed to add shipper"));
                }
            }

            //not valid
            return View(model);
        }

        public ActionResult ShipperEdit(int id)
        {
            ViewBag.PageTile = "Edit Shipper";
            ViewBag.Message = "";

            var shipperRepository = new AppCore.Shipper.ShipperRepository<AppCore.Shipper.Shipper>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var shipper = shipperRepository.FindById(id);

            if (shipper.IsEmpty())
            {
                return RedirectToAction("Error", "Home");
            }

            ShipperEditViewModel model = new ShipperEditViewModel()
            {
                ShipperID = shipper.ShipperID, 
                CompanyName = shipper.CompanyName, 
                Phone = shipper.Phone
            };

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ShipperEdit(ShipperEditViewModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var shipperRepository = new AppCore.Shipper.ShipperRepository<AppCore.Shipper.Shipper>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                var shipper = shipperRepository.FindById(model.ShipperID);

                shipper.CompanyName = model.CompanyName;
                shipper.Phone = model.Phone;

                var result = shipperRepository.Update(shipper);
                if (result)
                {
                    ViewBag.Message = "shipper updated!";
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to update shipper"));
            }

            //not valid
            return View(model);
        }

        public ActionResult ShipperDelete(int id = 0)
        {
            ViewBag.PageTile = "Delete Shipper";
            ViewBag.Message = "";

            var shipperRepository = new AppCore.Shipper.ShipperRepository<AppCore.Shipper.Shipper>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var shipper = shipperRepository.FindById(id);

            if (shipper.IsEmpty())
            {
                return HttpNotFound();
            }

            ShipperDeleteViewModel model = new ShipperDeleteViewModel()
            {
                ShipperID = shipper.ShipperID,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone
            };

            return View(model);
        }
        [HttpPost, ActionName("ShipperDelete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> ShipperDeleteConfirm(int id)
        {
            var shipperRepository = new AppCore.Shipper.ShipperRepository<AppCore.Shipper.Shipper>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var shipper = shipperRepository.FindById(id);

            if (!shipper.IsEmpty())
            {
                var result = shipperRepository.Delete(shipper);

                if (result)
                {
                    ViewBag.Message = "shipper deleted!";
                    var model = new ShipperDeleteViewModel();
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Shippers", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Customers
        public ActionResult Customers(string searchterm)
        {
            ViewBag.PageTitle = "Customers";
            ViewBag.Message = "";

            IQueryable<AppCore.Customer.Customer> customers;
            var customerRepository = new AppCore.Customer.CustomerRepository<AppCore.Customer.Customer>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (!String.IsNullOrEmpty(searchterm))
            {
                customers = customerRepository.GetCustomers().Where(c => c.CompanyName.ToLower()
                    .Contains(searchterm.ToLower()))
                    .OrderBy(c => c.CompanyName);
            }
            else
            {
                customers = customerRepository.GetCustomers().OrderBy(c => c.CompanyName);
            }

            return View(customers);
        }

        public ActionResult CustomerAdd(CustomerAddViewModel model)
        {
            ViewBag.PageTitle = "Add Customer";
            ViewBag.Message = "";

            //load dropdownlists
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");


            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerAdd(CustomerAddViewModel model, FormCollection form)
        {
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var customerRepository = new AppCore.Customer.CustomerRepository<AppCore.Customer.Customer>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                AppCore.Customer.Customer customer = new AppCore.Customer.Customer()
                {
                    CustomerID = model.CustomerID.ToUpper(),
                    CompanyName = model.CompanyName,
                    ContactName = model.ContactName,
                    ContactTitle = model.ContactTitle,
                    Address = model.Address,
                    City = model.City,
                    Region = model.Region,
                    Country = model.SelectedCountryID,
                    PostalCode = model.PostalCode,
                    Phone = model.Phone,
                    Fax = model.Fax
                };

                var result = customerRepository.Add(customer);
                if(result)
                {
                    ViewBag.Message = "customer added";
                    model = new CustomerAddViewModel();
                    model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");
                    ModelState.Clear();
                    return View(model);                        
                }
            }

            //not valid
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");

            return View(model);
        }

        public ActionResult CustomerEdit(string id)
        {
            ViewBag.PageTitle = "Edit Customer";
            ViewBag.Message = "";

            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var customerRepository = new AppCore.Customer.CustomerRepository<AppCore.Customer.Customer>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var customer = customerRepository.FindById(id);

            if (customer.IsEmpty())
            {
                return RedirectToAction("Error", "Home");
            }

            CustomerEditViewModel model = new CustomerEditViewModel()
            {
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = customer.ContactTitle,
                Address = customer.Address,
                City = customer.City,
                Region = customer.Region,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                Phone = customer.Phone,
                Fax = customer.Fax
            };

         
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
            model.SelectedCountryID = customer.Country;
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerEdit(CustomerEditViewModel model, FormCollection form)
        {
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var customerRepository = new AppCore.Customer.CustomerRepository<AppCore.Customer.Customer>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                var customer = customerRepository.FindById(model.CustomerID);
                if (customer.IsEmpty())
                {
                    return HttpNotFound();
                }

                customer.CompanyName = model.CompanyName;
                customer.ContactName = model.ContactName;
                customer.ContactTitle = model.ContactTitle;
                customer.Address = model.Address;
                customer.City = model.City;
                customer.Region = model.Region;
                customer.PostalCode = model.PostalCode;
                customer.Country = model.SelectedCountryID;
                customer.Phone = model.Phone;
                customer.Fax = model.Fax;

                var result = customerRepository.Update(customer);
                if (result)
                {
                    ViewBag.Message = "Customer Update";
                    model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
                    model.SelectedCountryID = customer.Country;
                    return View(model);
                }

                ModelState.AddModelError("", new Exception("failed to update customer"));
            }

            //not valid
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
            return View(model);
        }

        public ActionResult CustomerDelete(string id = "")
        {
            ViewBag.PageTitle = "Delete Customer";
            ViewBag.Message = "";

            var customerRepository = new AppCore.Customer.CustomerRepository<AppCore.Customer.Customer>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var customer = customerRepository.FindById(id);

            if (customer.IsEmpty())
            {
                return RedirectToAction("Error", "Home");
            }

            CustomerDeleteViewModel model = new CustomerDeleteViewModel()
            {
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = customer.ContactTitle,
                Address = customer.Address,
                City = customer.City,
                Region = customer.Region,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                Phone = customer.Phone,
                Fax = customer.Fax
            };

            return View(model);
        }
        [HttpPost, ActionName("CustomerDelete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerDeleteConfirm(string id)
        {
            var customerRepository = new AppCore.Customer.CustomerRepository<AppCore.Customer.Customer>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var customer = customerRepository.FindById(id);

            if (!customer.IsEmpty())
            {
                var result = customerRepository.Delete(customer);

                if (result)
                {
                    ViewBag.Message = "customer deleted!";
                    var model = new CustomerDeleteViewModel();
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Customers", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Employees
        public ActionResult Employees(string searchterm)
        {
            ViewBag.PageTitle = "Employees";
            ViewBag.Message = "";

            IQueryable<AppCore.Employee.Employee> employees;
            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (!String.IsNullOrEmpty(searchterm))
            {
                //search on multiple columns
                var predicate = PredicateBuilder.False<AppCore.Employee.Employee>();
                predicate = predicate.Or(e => e.LastName.ToLower().Contains(searchterm.ToLower())).Or(e=> e.FirstName.ToLower().Contains(searchterm.ToLower()));
                employees = employeeRepository.GetEmployees().Where(predicate);

                //employees = employeeRepository.GetEmployees().Where(e => e.LastName.ToLower().Contains(searchterm.ToLower())).OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
            }
            else
            {
                employees = employeeRepository.GetEmployees().OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
            }

            return View(employees);

        }

        public ActionResult EmployeeAdd(EmployeeAddViewModel model)
        {
            ViewBag.PageTitle = "Add Employee";
            ViewBag.Message = "";

            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");

            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            model.Employees = new SelectList(employeeRepository.GetEmployees().OrderBy(e => e.LastName).OrderBy(e => e.FirstName), "EmployeeID", "LastName");

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeeAdd(EmployeeAddViewModel model, FormCollection form, HttpPostedFileBase NewPhoto)
        {
            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                var employee = new AppCore.Employee.Employee()
                {
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    Title = model.Title,
                    TitleOfCourtesy = model.TitleOfCourtesy,
                    BirthDate = model.BirthDate,
                    HireDate = model.HireDate,
                    Address = model.Address,
                    City = model.City,
                    Region = model.Region,
                    PostalCode = model.PostalCode,
                    Country = model.SelectedCountryID,
                    HomePhone = model.HomePhone,
                    Extension = model.Extension,
                    //Photo = "", 
                    Notes = model.Notes,
                    ReportsTo = model.SelectedEmployeeID,
                    PhotoPath = model.PhotoPath,
                    Salary = model.Salary
                };

                //get picture
                if (NewPhoto != null && NewPhoto.ContentLength > 0)
                {
                    string filetype = NewPhoto.ContentType;
                    Int32 length = NewPhoto.ContentLength;
                    byte[] tempImage = new byte[length];
                    NewPhoto.InputStream.Read(tempImage, 0, length);
                    employee.Photo = tempImage;
                    model.Photo = tempImage;
                }

                var result = employeeRepository.Add(employee);
                if (result)
                {
                    ViewBag.Message = "Employee added!";
                    model = new EmployeeAddViewModel();
                    model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");
                    model.Employees = new SelectList(employeeRepository.GetEmployees().OrderBy(e => e.LastName).OrderBy(e => e.FirstName), "EmployeeID", "LastName");
                    
                    ModelState.Clear();
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to add employee"));
                //AddErrors(result);
            }

            //not valid
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");
            model.Employees = new SelectList(employeeRepository.GetEmployees().OrderBy(e => e.LastName).OrderBy(e => e.FirstName), "EmployeeID", "LastName");

            return View(model);
        }

        public ActionResult EmployeeEdit(int id)
        {
            ViewBag.PageTitle = "Edit Employee";
            ViewBag.Message = "";

            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var employee = employeeRepository.FindById(id);

            if (employee.IsEmpty())
            {
                //category did not load go to error page
                return RedirectToAction("Error", "Home");
            }

            EmployeeEditViewModel model = new EmployeeEditViewModel();
            model.EmployeeID = employee.EmployeeID;
            model.LastName = employee.LastName;
            model.FirstName = employee.FirstName;
            model.Title = employee.Title;
            model.TitleOfCourtesy = employee.TitleOfCourtesy;
            model.BirthDate = employee.BirthDate;
            model.HireDate = employee.HireDate;
            model.Address = employee.Address;
            model.City = employee.City;
            model.Region = employee.Region;
            model.PostalCode = employee.PostalCode;
            model.Country = employee.Country;
            model.HomePhone = employee.HomePhone;
            model.Extension = employee.Extension;
            model.Photo = employee.Photo;
            model.Notes = employee.Notes;
            model.ReportsTo = employee.EmployeeID;
            model.PhotoPath = employee.PhotoPath;
            model.Salary = employee.Salary;

       
            model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name", "CAN");
            model.SelectedCountryID = employee.Country;
            model.Employees = new SelectList(employeeRepository.GetEmployees().OrderBy(e => e.LastName).OrderBy(e => e.FirstName), "EmployeeID", "LastName");
            if(model.ReportsTo.HasValue) model.SelectedEmployeeID = (int)model.ReportsTo;
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeeEdit(EmployeeEditViewModel model, FormCollection form, HttpPostedFileBase NewPhoto)
        {
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                var employee = employeeRepository.FindById(model.EmployeeID); 
                employee.LastName = model.LastName;
                employee.FirstName = model.FirstName;
                employee.Title = model.Title;
                employee.TitleOfCourtesy = model.TitleOfCourtesy;
                employee.BirthDate = model.BirthDate;
                employee.HireDate = model.HireDate;
                employee.Address = model.Address;
                employee.City = model.City;
                employee.Region = model.Region;
                employee.PostalCode = model.PostalCode;
                employee.Country = model.SelectedCountryID;
                employee.HomePhone = model.HomePhone;
                employee.Extension = model.Extension;
                employee.Notes = model.Notes;
                employee.ReportsTo = model.SelectedEmployeeID;
                employee.PhotoPath = model.PhotoPath;
                employee.Salary = model.Salary;
                //get picture
                if (NewPhoto != null && NewPhoto.ContentLength > 0)
                {
                    string filetype = NewPhoto.ContentType;
                    Int32 length = NewPhoto.ContentLength;
                    byte[] tempImage = new byte[length];
                    NewPhoto.InputStream.Read(tempImage, 0, length);
                    employee.Photo = tempImage;
                    model.Photo = tempImage;
                }
                else
                {
                    model.Photo = employee.Photo;
                }

                //update category        
                var result = employeeRepository.Update(employee);
                if (result)
                {
                    ViewBag.Message = "employee updated!";
                    model.Countries = new SelectList(countryRepository.GetCountries().OrderBy(c => c.Name), "Code", "Name");
                    model.SelectedCountryID = employee.Country;
                    model.Employees = new SelectList(employeeRepository.GetEmployees().OrderBy(e => e.LastName).OrderBy(e => e.FirstName), "EmployeeID", "LastName");
                    if(employee.ReportsTo.HasValue) model.SelectedEmployeeID = (int)employee.ReportsTo;
                    return View(model);
                }
                ModelState.AddModelError("", new Exception("failed to update employee"));
                //AddErrors(result);
            }
            return View(model);
        }
        
        public ActionResult EmployeeDelete(int id)
        {
            ViewBag.PageTitle = "Delete Employee";
            ViewBag.Message = "";

            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var employee = employeeRepository.FindById(id);
           
            if (employee.IsEmpty())
            {
                return RedirectToAction("Error", "Home");
            }

            AppCore.Employee.Employee manager = new AppCore.Employee.Employee(); 
            if (employee.ReportsTo.HasValue) manager = employeeRepository.FindById((int)employee.ReportsTo);

            EmployeeDeleteViewModel model = new EmployeeDeleteViewModel();
            model.LastName = employee.LastName;
            model.FirstName = employee.FirstName;
            model.Title = employee.Title;
            model.TitleOfCourtesy = employee.TitleOfCourtesy;
            model.BirthDate = employee.BirthDate;
            model.HireDate = employee.HireDate;
            model.Address = employee.Address;
            model.City = employee.City;
            model.Region = employee.Region;
            model.PostalCode = employee.PostalCode;
            model.Country = employee.Country;
            model.HomePhone = employee.HomePhone;
            model.Extension = employee.Extension;
            model.Photo = employee.Photo;
            model.Notes = employee.Notes;
            model.ReportsTo = employee.EmployeeID;
            if (!manager.IsEmpty()) model.ReportsToName = string.Format("{0} {1}", manager.FirstName, manager.LastName);
            model.PhotoPath = employee.PhotoPath;
            model.Salary = employee.Salary;

            return View(model);
        }
        [HttpPost, ActionName("EmployeeDelete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeeDeleteConfirm(int id)
        {
            var employeeRepository = new AppCore.Employee.EmployeeRepository<AppCore.Employee.Employee>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var employee = employeeRepository.FindById(id);

            if (!employee.IsEmpty())
            {
                var result = employeeRepository.Delete(employee);

                if (result)
                {
                    ViewBag.Message = "employee deleted!";
                    var model = new EmployeeDeleteViewModel();
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Employees", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Regions
        public ActionResult Regions(string searchterm)
        {
            ViewBag.PageTitle = "Regions";
            ViewBag.Message = "";

            IQueryable<AppCore.Region.Region> regions;
            var regionRepository = new AppCore.Region.RegionRepository<AppCore.Region.Region>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (string.IsNullOrEmpty(searchterm))
            {
                regions = regionRepository.GetRegions().OrderBy(r => r.RegionDescription);
            }
            else
            {
                regions = regionRepository.GetRegions().Where(r => r.RegionDescription.ToLower()
                    .Contains(searchterm.ToLower()))
                    .OrderBy(r => r.RegionDescription);
            }

            return View(regions);
        }

        public ActionResult RegionAdd()
        {
            ViewBag.PageTitle = "Add Region";
            ViewBag.Message = "";

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> RegionAdd(RegionAddViewModel model, FormCollection form)
        {
            var regionRepository = new AppCore.Region.RegionRepository<AppCore.Region.Region>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            if (ModelState.IsValid)
            {
                AppCore.Region.Region region = new AppCore.Region.Region()
                {
                    RegionDescription = model.RegionDescription
                };

                var result = await regionRepository.AddAsync(region);
                if (result == true)
                {
                    ViewBag.Message = "region added";
                    model = new RegionAddViewModel(); 
                    ModelState.Clear();
                    return View(model);
                }
            }

            //not valid 
            return View(model);
        }

        public ActionResult RegionEdit(int id)
        {
            ViewBag.PageTitle = "Edit Region";
            ViewBag.Message = "";

            var regionRepository = new AppCore.Region.RegionRepository<AppCore.Region.Region>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var region = regionRepository.FindById(id);

            if (region.IsEmpty())
            {
                return HttpNotFound();
            }

            RegionEditViewModel model = new RegionEditViewModel()
            {
                RegionID = region.RegionID,
                RegionDescription = region.RegionDescription
            };

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> RegionEdit(RegionEditViewModel model, FormCollection form)
        {
            var regionRepository = new AppCore.Region.RegionRepository<AppCore.Region.Region>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (ModelState.IsValid)
            {
                var region = regionRepository.FindById(model.RegionID);

                if (region.IsEmpty())
                {
                    return HttpNotFound();
                }

                region.RegionDescription = model.RegionDescription;
                var result = await regionRepository.UpdateAsync(region);
                if (result)
                {
                    ViewBag.Message = "Region Update";
                    return View(model);
                }

                ModelState.AddModelError("", new Exception("failed to update region"));
            }

            //is not valid
            return View(model);
        }

        public ActionResult RegionDelete(int id)
        {
            ViewBag.PageTitle = "Delete Region";
            ViewBag.Message = "";

            var regionRepository = new AppCore.Region.RegionRepository<AppCore.Region.Region>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var region = regionRepository.FindById(id);

            if (region.IsEmpty())
            {
                return HttpNotFound();
            }

            RegionDeleteViewModel model = new RegionDeleteViewModel()
            {
                RegionID = region.RegionID,
                RegionDescription = region.RegionDescription
            };

            return View(model);
        }
        [HttpPost, ActionName("RegionDelete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> RegionDeleteConfirm(int id, FormCollection form)
        {
            var regionRepository = new AppCore.Region.RegionRepository<AppCore.Region.Region>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var region = regionRepository.FindById(id);

            if (!region.IsEmpty())
            {
                var result = await regionRepository.DeleteAsync(region);

                if (result)
                {
                    ViewBag.Message = "region deleted!";
                    var model = new RegionDeleteViewModel();
                    model.ShowDeleteButton = false;
                    //ModelState.Clear();
                    return View(model);
                }

                return RedirectToAction("Regions", "Admin");
            }

            // Not Found
            return HttpNotFound();
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private static List<SelectListItem> SelectListForGender(string defaultValue = null)
        {
            List<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem
                {
                    Text = "Male",
                    Value = "M"
                });
            genders.Add(new SelectListItem
                {
                    Text = "Female",
                    Value = "F",
                });


            SelectList lst;
            if (defaultValue != null)
                lst = new SelectList(genders, "Text", "Value", defaultValue);
            else
                lst = new SelectList(genders, "Text", "Value");

            return genders;
        }
        private static SelectList SelectListForCountries(string defaultValue = null)
        {
            var countryRepository = new AppCore.Country.CountryRepository<AppCore.Country.Country>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var countries = countryRepository.GetCountries().OrderBy(c => c.Name);
            SelectList lst;
            if (defaultValue != null)
                lst = new SelectList(countries, "Code", "Name", defaultValue);
            else
                lst = new SelectList(countries, "Code", "Name");

            return lst;
        }
        private static SelectList SelectListForLanguages(string defaultValue = null)
        {
            var langRepository = new AppCore.Lanaguage.LanguageRepository<AppCore.Lanaguage.Language>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var langs = langRepository.GetLanguages();
            SelectList lst;
            if (defaultValue != null)
                lst = new SelectList(langs, "code", "Name_EN", defaultValue);
            else
                lst = new SelectList(langs, "code", "Name_EN");

            return lst;
        }
        #endregion
    }
}


