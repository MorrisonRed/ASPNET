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
        public async Task<ActionResult> DeleteDeleteConfirmed(string id)
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
            var countries = countryRepository.GetCountries();
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


