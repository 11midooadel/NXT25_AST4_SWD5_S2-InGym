using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService,SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountViewModel accountView)
        {
            if (!ModelState.IsValid) return View(accountView);
            var user = _accountService.ValidateUser(accountView);

            if(user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(accountView);
            }
            var Result = _signInManager.PasswordSignInAsync(user, accountView.Password, accountView.RememberMe, false).Result;

            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your account not Allowed");
            if (Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account is Locked");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(accountView);

        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Access Denied
        public ActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
