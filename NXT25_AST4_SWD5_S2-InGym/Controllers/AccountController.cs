using Microsoft.AspNetCore.Mvc;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;


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
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // هنكتب كود تسجيل الدخول بعدين

            return RedirectToAction("Index", "Home");
        }

        // ================= Register =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // التأكد أن الإيميل غير موجود
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

                // مؤقتاً (هنعمل Hash بعدين)
                PasswordHash = model.Password,

                Role = "Member"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}