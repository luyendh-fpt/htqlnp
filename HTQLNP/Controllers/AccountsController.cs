using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using HTQLNP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace HTQLNP.Controllers
{
    public class AccountsController : Controller
    {
        private MyDBContext dbContext = new MyDBContext();
        private RoleManager<AppRole> roleManager;
        private UserManager<Account> userManager;

        public AccountsController()
        {
            var roleStore = new RoleStore<AppRole>(dbContext);
            roleManager = new RoleManager<AppRole>(roleStore);
            var userStore = new UserStore<Account>(dbContext);
            userManager = new UserManager<Account>(userStore);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddRole(string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new AppRole(roleName));
            }
            return Redirect("/Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddUserToRole(string accountId, string roleName)
        {
           
            userManager.AddToRole(accountId, roleName);
            return Redirect("/Home");
        }

        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessRegister(string username, string password)
        {
            var account = new Account()
            {
                UserName = username,
                Birthday = DateTime.Now
            };

            IdentityResult result = userManager.Create(account, password);
            Debug.WriteLine("@@@"+result.Succeeded);
            if (result.Succeeded)
            {
                Debug.WriteLine("@@@" + account.Id);
                userManager.AddToRole(account.Id, "User");
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(
                    account, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                return Redirect("/Home");
            }
            return View("Login");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult ProcessLogin(string username, string password)
        {
            var user = userManager.Find(username, password);
            if (user!=null)
            {
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(
                    user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                return Redirect("/Home");
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            var authenticationManager = System.Web.HttpContext.Current
                .GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return View("Login");
        }
    }
}