using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    public class AccountController : Controller
    {
        private readonly GymDbContext _context;

        public AccountController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Login =================

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Member"))
                    return RedirectToAction("Dashboard", "Home");

                if (User.IsInRole("Coach"))
                    return RedirectToAction("Dashboard", "Coach");

                if (User.IsInRole("GymManager"))
                    return RedirectToAction("Dashboard", "GymManager");

                if (User.IsInRole("Admin"))
                    return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User? user = _context.Users
                                 .FirstOrDefault(u => u.Email == model.Email);

            if (user == null || user.PasswordHash != model.Password)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            // ================= Member =================

            if (user.Role == "Member")
            {
                Member? member = _context.Members
                                         .FirstOrDefault(m => m.UserID == user.UserID);

                if (member == null)
                    return RedirectToAction("CompleteProfile", "Member");

                bool joinedGym = _context.GymMembers
                                         .Any(gm => gm.MemberID == member.MemberID);

                if (!joinedGym)
                    return RedirectToAction("ChooseGym", "Member");

                return RedirectToAction("Dashboard", "Home");
            }

            // ================= Coach =================

            if (user.Role == "Coach")
            {
                return RedirectToAction("Dashboard", "Coach");
            }

            // ================= Gym Manager =================

            if (user.Role == "GymManager")
            {
                return RedirectToAction("Dashboard", "GymManager");
            }

            // ================= Admin =================

            if (user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return RedirectToAction("Login");
        }

        // ================= Register =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                Role = "Member"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Account created successfully. Please login.";

            return RedirectToAction(nameof(Login));
        }

        // ================= Logout =================

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
    }
}