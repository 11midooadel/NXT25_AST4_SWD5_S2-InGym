using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Coach")]
    public class WorkoutController : Controller
    {
        private readonly GymDbContext _context;

        public WorkoutController(GymDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var plans = _context.WorkoutPlans
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return View(plans);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(WorkoutPlanViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            WorkoutPlan plan = new WorkoutPlan
            {
                Goal = model.Goal,
                CreationDate = DateTime.Now,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive
            };

            _context.WorkoutPlans.Add(plan);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}