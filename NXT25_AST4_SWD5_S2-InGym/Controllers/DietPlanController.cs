using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize]
    public class DietPlanController : Controller
    {
        private readonly GymDbContext _context;

        public DietPlanController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Coach : View Member Diet Plans =================

        [Authorize(Roles = "Coach")]
        public IActionResult DietPlans(int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == memberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            var dietPlans = _context.MemberDietaryPlans
                .Where(mdp => mdp.MemberID == memberId)
                .Include(mdp => mdp.DietaryPlan)
                .Select(mdp => new DietPlanViewModel
                {
                    DietID = mdp.DietaryPlan.DietID,
                    CalorieTarget = mdp.DietaryPlan.CalorieTarget,
                    ProteinTarget = mdp.DietaryPlan.ProteinTarget,
                    Notes = mdp.DietaryPlan.Notes,
                    CreationDate = mdp.DietaryPlan.CreationDate,
                    MemberId = memberId,
                    MemberName = member.User.FirstName + " " + member.User.LastName
                })
                .OrderByDescending(dp => dp.CreationDate)
                .ToList();

            return View(dietPlans);
        }

        // ================= Coach : Create Diet Plan =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult CreateDietPlan(int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == memberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            var viewModel = new CreateDietPlanViewModel
            {
                MemberId = memberId,
                MemberName = member.User.FirstName + " " + member.User.LastName
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDietPlan(CreateDietPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .FirstOrDefault(m => m.MemberID == model.MemberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            // Create diet plan
            var dietPlan = new DietaryPlan
            {
                CalorieTarget = model.CalorieTarget,
                ProteinTarget = model.ProteinTarget,
                Notes = model.Notes,
                CreationDate = DateTime.Now
            };

            _context.DietaryPlans.Add(dietPlan);
            _context.SaveChanges();

            // Link member to diet plan
            var memberDietPlan = new MemberDietaryPlan
            {
                MemberID = model.MemberId,
                DietID = dietPlan.DietID
            };

            _context.MemberDietaryPlans.Add(memberDietPlan);
            _context.SaveChanges();

            TempData["Success"] = "Diet plan created successfully.";
            return RedirectToAction("DietPlans", new { memberId = model.MemberId });
        }

        // ================= Coach : Edit Diet Plan =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult EditDietPlan(int dietId, int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == memberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            var dietPlan = _context.DietaryPlans
                .FirstOrDefault(d => d.DietID == dietId);

            if (dietPlan == null)
                return NotFound();

            var viewModel = new EditDietPlanViewModel
            {
                DietId = dietPlan.DietID,
                CalorieTarget = dietPlan.CalorieTarget,
                ProteinTarget = dietPlan.ProteinTarget,
                Notes = dietPlan.Notes,
                MemberId = memberId,
                MemberName = member.User.FirstName + " " + member.User.LastName
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDietPlan(EditDietPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .FirstOrDefault(m => m.MemberID == model.MemberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            var dietPlan = _context.DietaryPlans
                .FirstOrDefault(d => d.DietID == model.DietId);

            if (dietPlan == null)
                return NotFound();

            dietPlan.CalorieTarget = model.CalorieTarget;
            dietPlan.ProteinTarget = model.ProteinTarget;
            dietPlan.Notes = model.Notes;

            _context.SaveChanges();

            TempData["Success"] = "Diet plan updated successfully.";
            return RedirectToAction("DietPlans", new { memberId = model.MemberId });
        }

        // ================= Coach : Delete Diet Plan =================

        [Authorize(Roles = "Coach")]
        [HttpPost]
        public IActionResult DeleteDietPlan(int dietId, int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .FirstOrDefault(m => m.MemberID == memberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            var memberDietPlan = _context.MemberDietaryPlans
                .FirstOrDefault(mdp => mdp.DietID == dietId && mdp.MemberID == memberId);

            if (memberDietPlan == null)
                return NotFound();

            var dietPlan = _context.DietaryPlans
                .FirstOrDefault(d => d.DietID == dietId);

            if (dietPlan != null)
            {
                _context.DietaryPlans.Remove(dietPlan);
            }

            _context.MemberDietaryPlans.Remove(memberDietPlan);
            _context.SaveChanges();

            TempData["Success"] = "Diet plan deleted successfully.";
            return RedirectToAction("DietPlans", new { memberId });
        }

        // ================= Member : View Own Diet Plans =================

        [Authorize(Roles = "Member")]
        public IActionResult MyDietPlans()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var dietPlans = _context.MemberDietaryPlans
                .Where(mdp => mdp.MemberID == member.MemberID)
                .Include(mdp => mdp.DietaryPlan)
                .Select(mdp => new DietPlanViewModel
                {
                    DietID = mdp.DietaryPlan.DietID,
                    CalorieTarget = mdp.DietaryPlan.CalorieTarget,
                    ProteinTarget = mdp.DietaryPlan.ProteinTarget,
                    Notes = mdp.DietaryPlan.Notes,
                    CreationDate = mdp.DietaryPlan.CreationDate,
                    MemberId = member.MemberID,
                    MemberName = member.User.FirstName + " " + member.User.LastName
                })
                .OrderByDescending(dp => dp.CreationDate)
                .ToList();

            return View("MyDietPlans", dietPlans);
        }
    }
}
