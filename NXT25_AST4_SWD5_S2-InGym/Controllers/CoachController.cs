using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Member")]
    public class CoachController : Controller
    {
        private readonly GymDbContext _context;

        public CoachController(GymDbContext context)
        {
            _context = context;
        }

        // ===================== Choose Gym Coach =====================

        [HttpGet]
        public IActionResult ChooseGymCoach()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                                 .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            // الجيم الذي لديه اشتراك فعال
            var subscription = _context.MemberGymSubs
                                       .FirstOrDefault(x => x.MemberID == member.MemberID &&
                                                            x.IsActive);

            if (subscription == null)
                return RedirectToAction("ChooseSubscription", "Subscription");

            var coaches = _context.GymCoaches
                                  .Where(gc => gc.GymID == subscription.GymID)
                                  .Include(gc => gc.Coach)
                                  .ThenInclude(c => c.User)
                                  .Select(gc => new CoachViewModel
                                  {
                                      CoachID = gc.CoachID,
                                      FullName = gc.Coach.User.FirstName + " " + gc.Coach.User.LastName,
                                      Email = gc.Coach.User.Email,
                                      Speciality = gc.Coach.Speciality,
                                      Rating = gc.Coach.Rating
                                  })
                                  .ToList();

            return View(coaches);
        }

        // ===================== Select Coach =====================

        [HttpGet]
        public IActionResult SelectCoach(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                                 .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var coach = _context.Coaches.FirstOrDefault(c => c.CoachID == id);

            if (coach == null)
                return NotFound();

            member.CoachID = coach.CoachID;

            _context.SaveChanges();

            TempData["Success"] = "Gym coach selected successfully.";

            return RedirectToAction("Dashboard", "Home");
        }
    }
}