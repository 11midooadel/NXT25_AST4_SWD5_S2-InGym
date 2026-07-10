using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    public class GymController : Controller
    {
        private readonly GymDbContext _context;

        public GymController(GymDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var gyms = _context.Gyms
                               .Include(g => g.GymLocations)
                               .ToList();

            return View(gyms);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GymViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Gym gym = new Gym
            {
                GymName = model.GymName,
                RegistrationDate = model.RegistrationDate
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

            return RedirectToAction(nameof(Index));
        }
    }
}