using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Member")]
    public class PrivateTrainerController : Controller
    {
        private readonly GymDbContext _context;

        public PrivateTrainerController(GymDbContext context)
        {
            _context = context;
        }

        // عرض جميع المدربين
        public IActionResult ChooseCoach()
        {
            var coaches = _context.Coaches
                .Include(c => c.User)
                .Select(c => new PrivateCoachViewModel
                {
                    CoachID = c.CoachID,
                    FullName = c.User.FirstName + " " + c.User.LastName,
                    Email = c.User.Email,
                    Speciality = c.Speciality,
                    Rating = c.Rating
                })
                .ToList();

            return View(coaches);
        }

        // عرض الباقات
        public IActionResult ChoosePackage(int coachId)
        {
            ViewBag.CoachID = coachId;

            var packages = _context.PrivateSubs.ToList();

            return View(packages);
        }

        // الاشتراك
        public IActionResult Subscribe(int coachId, int packageId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members.FirstOrDefault(x => x.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            if (_context.MemberPrivateSubs.Any(x => x.MemberID == member.MemberID && x.IsActive))
            {
                TempData["Error"] = "You already have an active private subscription.";
                return RedirectToAction("Dashboard", "Home");
            }

            var package = _context.PrivateSubs.FirstOrDefault(x => x.PrivateSubID == packageId);

            if (package == null)
                return NotFound();

            MemberPrivateSub sub = new MemberPrivateSub
            {
                MemberID = member.MemberID,
                CoachID = coachId,
                PrivateSubID = packageId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            _context.MemberPrivateSubs.Add(sub);

            _context.SaveChanges();

            TempData["Success"] = "Private Trainer Added Successfully.";

            return RedirectToAction("Dashboard", "Home");
        }
    }
}