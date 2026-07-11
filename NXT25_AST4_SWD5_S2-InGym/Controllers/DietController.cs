using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Coach")]
    public class DietController : Controller
    {
        private readonly GymDbContext _context;

        public DietController(GymDbContext context)
        {
            _context = context;
        }

        // ================= View Diet Plans =================

        [HttpGet]
        public IActionResult DietPlans(int memberId)
        {
            ViewBag.MemberID = memberId;

            var plans = _context.MemberDietaryPlans
                .Where(x => x.MemberID == memberId)
                .Include(x => x.DietaryPlan)
                .Select(x => x.DietaryPlan)
                .ToList();

            return View(plans);
        }

        // ================= Create Diet Plan =================

        [HttpGet]
        public IActionResult CreateDietPlan(int memberId)
        {
            ViewBag.MemberID = memberId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDietPlan(int memberId, DietaryPlan model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MemberID = memberId;
                return View(model);
            }

            var dietPlan = new DietaryPlan
            {
                Goal = model.Goal,
                Description = model.Description,
                Calories = model.Calories,
                Protein = model.Protein,
                Carbs = model.Carbs,
                Fats = model.Fats,
                CreationDate = DateTime.Now,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = true
            };

            _context.DietaryPlans.Add(dietPlan);
            _context.SaveChanges();

            var memberPlan = new MemberDietaryPlan
            {
                MemberID = memberId,
                DietID = dietPlan.DietID
            };

            _context.MemberDietaryPlans.Add(memberPlan);
            _context.SaveChanges();

            TempData["Success"] = "Diet Plan Created Successfully.";

            return RedirectToAction(nameof(DietPlans), new { memberId });
        }

        // ================= View Diet Plan Details =================

        [HttpGet]
        public IActionResult ViewDietPlan(int id)
        {
            var plan = _context.DietaryPlans
                .FirstOrDefault(x => x.DietID == id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }

        // ================= Edit Diet Plan =================

        [HttpGet]
        public IActionResult EditDietPlan(int id)
        {
            var plan = _context.DietaryPlans
                .FirstOrDefault(x => x.DietID == id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDietPlan(int id, DietaryPlan model)
        {
            var plan = _context.DietaryPlans
                .FirstOrDefault(x => x.DietID == id);

            if (plan == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            plan.Goal = model.Goal;
            plan.Description = model.Description;
            plan.Calories = model.Calories;
            plan.Protein = model.Protein;
            plan.Carbs = model.Carbs;
            plan.Fats = model.Fats;
            plan.StartDate = model.StartDate;
            plan.EndDate = model.EndDate;
            plan.IsActive = model.IsActive;

            _context.SaveChanges();

            TempData["Success"] = "Diet Plan Updated Successfully.";

            var memberDiet = _context.MemberDietaryPlans
                .FirstOrDefault(x => x.DietID == id);

            return RedirectToAction(nameof(DietPlans), new { memberId = memberDiet?.MemberID });
        }

        // ================= Delete Diet Plan =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDietPlan(int id)
        {
            var plan = _context.DietaryPlans
                .FirstOrDefault(x => x.DietID == id);

            if (plan == null)
                return NotFound();

            var memberDiet = _context.MemberDietaryPlans
                .FirstOrDefault(x => x.DietID == id);

            int memberId = memberDiet?.MemberID ?? 0;

            _context.MemberDietaryPlans.RemoveRange(
                _context.MemberDietaryPlans.Where(x => x.DietID == id)
            );

            _context.DietaryPlans.Remove(plan);
            _context.SaveChanges();

            TempData["Success"] = "Diet Plan Deleted Successfully.";

            return RedirectToAction(nameof(DietPlans), new { memberId });
        }
    }
}
