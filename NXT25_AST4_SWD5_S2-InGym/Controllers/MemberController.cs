using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        private readonly GymDbContext _context;

        public MemberController(GymDbContext context)
        {
            _context = context;
        }

        // ================= GET =================

        [HttpGet]
        public IActionResult CompleteProfile()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // لو العضو كمل البروفايل قبل كده
            if (_context.Members.Any(m => m.UserID == userId))
            {
                return RedirectToAction("Dashboard", "Home");
            }

            return View();
        }

        // ================= POST =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteProfile(MemberProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // التأكد إنه مش مسجل قبل كده
            if (_context.Members.Any(m => m.UserID == userId))
            {
                TempData["Error"] = "Profile already completed.";
                return RedirectToAction("Dashboard", "Home");
            }

            Member member = new Member
            {
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Height = model.Height,
                Weight = model.Weight,
                JoinDate = DateTime.Now,
                Goals = model.Goals,
                PhysRestrictions = model.PhysRestrictions,
                ChronicDiseases = model.ChronicDiseases,
                IsActive = true,
                UserID = userId,
                CoachID = null
            };

            _context.Members.Add(member);
            _context.SaveChanges();

            TempData["Success"] = "Profile completed successfully.";

            return RedirectToAction("Dashboard", "Home");
        }
    }
}