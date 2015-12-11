using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ASPNET.Models;
using System.Collections.Generic;

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

        //GET: Roles
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
                    model = null;
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

        #endregion
    }
}


