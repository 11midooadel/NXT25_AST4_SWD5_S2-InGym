using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels.Admin;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly GymDbContext _context;

        public AdminController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Dashboard =================

        public IActionResult Dashboard()
        {
            ViewBag.Gyms = _context.Gyms.Count();
            ViewBag.Members = _context.Members.Count();
            ViewBag.Coaches = _context.Coaches.Count();
            ViewBag.Managers = _context.GymManagers.Count();

            return View();
        }

        // ================= Add Gym =================

        [HttpGet]
        public IActionResult AddGym()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGym(AddGymViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Gym gym = new Gym
            {
                GymName = model.GymName,
                RegistrationDate = DateTime.Now
            };

            _context.Gyms.Add(gym);
            _context.SaveChanges();

            GymLocation location = new GymLocation
            {
                GymID = gym.GymID,
                Location = model.Location
            };

            _context.GymLocations.Add(location);
            _context.SaveChanges();

            TempData["Success"] = "Gym added successfully.";

            return RedirectToAction(nameof(Gyms));
        }

        // ================= View Gyms =================

        public IActionResult Gyms()
        {
            var gyms = _context.Gyms
                .Include(g => g.GymLocations)
                .Include(g => g.GymManagers)
                .ToList();

            return View(gyms);
        }

        // ================= Add Gym Manager =================

        [HttpGet]
        public IActionResult AddGymManager()
        {
            AddGymManagerViewModel model = new AddGymManagerViewModel();

            model.Gyms = _context.Gyms
                .Select(g => new SelectListItem
                {
                    Value = g.GymID.ToString(),
                    Text = g.GymName
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGymManager(AddGymManagerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Gyms = _context.Gyms
                    .Select(g => new SelectListItem
                    {
                        Value = g.GymID.ToString(),
                        Text = g.GymName
                    })
                    .ToList();

                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");

                model.Gyms = _context.Gyms
                    .Select(g => new SelectListItem
                    {
                        Value = g.GymID.ToString(),
                        Text = g.GymName
                    })
                    .ToList();

                return View(model);
            }

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                Role = "GymManager"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            GymManager manager = new GymManager
            {
                UserID = user.UserID,
                Salary = model.Salary,
                HireDate = model.HireDate,
                GymID = model.GymID
            };

            _context.GymManagers.Add(manager);
            _context.SaveChanges();

            TempData["Success"] = "Gym Manager added successfully.";

            return RedirectToAction(nameof(Dashboard));
        }
    }
}