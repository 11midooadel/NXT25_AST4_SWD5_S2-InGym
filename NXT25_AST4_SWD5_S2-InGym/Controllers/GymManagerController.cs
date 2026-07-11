using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "GymManager")]
    public class GymManagerController : Controller
    {
        private readonly GymDbContext _context;

        public GymManagerController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Dashboard =================

        public IActionResult Dashboard()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .Include(m => m.Gym)
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Index", "Home");

            // Get gym-specific stats
            var gymCoaches = _context.GymCoaches
                .Where(gc => gc.GymID == manager.GymID)
                .Count();

            var gymMembers = _context.GymMembers
                .Where(gm => gm.GymID == manager.GymID)
                .Count();

            var activeSubscriptions = _context.MemberGymSubs
                .Where(mgs => mgs.GymID == manager.GymID && mgs.IsActive)
                .Count();

            var todayAttendance = _context.Attendances
                .Where(a => a.CheckInTime.Date == DateTime.Today)
                .Count();

            ViewBag.GymName = manager.Gym.GymName;
            ViewBag.GymID = manager.GymID;
            ViewBag.TotalCoaches = gymCoaches;
            ViewBag.TotalMembers = gymMembers;
            ViewBag.ActiveSubscriptions = activeSubscriptions;
            ViewBag.TodayAttendance = todayAttendance;

            return View();
        }

        // ================= Manage Coaches =================

        [HttpGet]
        public IActionResult Coaches()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var coaches = _context.GymCoaches
                .Where(gc => gc.GymID == manager.GymID)
                .Include(gc => gc.Coach)
                .ThenInclude(c => c.User)
                .Select(gc => gc.Coach)
                .ToList();

            ViewBag.GymID = manager.GymID;
            return View(coaches);
        }

        // ================= Add Coach =================

        [HttpGet]
        public IActionResult AddCoach()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var availableCoaches = _context.Coaches
                .Include(c => c.User)
                .Where(c => !_context.GymCoaches.Any(gc => gc.CoachID == c.CoachID && gc.GymID == manager.GymID))
                .Select(c => new SelectListItem
                {
                    Value = c.CoachID.ToString(),
                    Text = c.User.FirstName + " " + c.User.LastName + " (" + c.Speciality + ")"
                })
                .ToList();

            ViewBag.Coaches = availableCoaches;
            ViewBag.GymID = manager.GymID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCoach(int coachId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var coach = _context.Coaches.FirstOrDefault(c => c.CoachID == coachId);

            if (coach == null)
                return NotFound();

            if (_context.GymCoaches.Any(gc => gc.CoachID == coachId && gc.GymID == manager.GymID))
            {
                TempData["Error"] = "Coach is already assigned to this gym.";
                return RedirectToAction(nameof(Coaches));
            }

            var gymCoach = new GymCoach
            {
                GymID = manager.GymID,
                CoachID = coachId
            };

            _context.GymCoaches.Add(gymCoach);
            _context.SaveChanges();

            TempData["Success"] = "Coach added to gym successfully.";

            return RedirectToAction(nameof(Coaches));
        }

        // ================= Remove Coach =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveCoach(int coachId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var gymCoach = _context.GymCoaches
                .FirstOrDefault(gc => gc.CoachID == coachId && gc.GymID == manager.GymID);

            if (gymCoach == null)
                return NotFound();

            _context.GymCoaches.Remove(gymCoach);
            _context.SaveChanges();

            TempData["Success"] = "Coach removed from gym successfully.";

            return RedirectToAction(nameof(Coaches));
        }

        // ================= Manage Members =================

        [HttpGet]
        public IActionResult Members()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var members = _context.GymMembers
                .Where(gm => gm.GymID == manager.GymID)
                .Include(gm => gm.Member)
                .ThenInclude(m => m.User)
                .Select(gm => gm.Member)
                .ToList();

            return View(members);
        }

        // ================= View Member Details =================

        [HttpGet]
        public IActionResult MemberDetails(int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var member = _context.Members
                .Include(m => m.User)
                .Include(m => m.Coach)
                .ThenInclude(c => c.User)
                .FirstOrDefault(m => m.MemberID == memberId);

            if (member == null)
                return NotFound();

            // Verify member is in this manager's gym
            var gymMember = _context.GymMembers
                .FirstOrDefault(gm => gm.MemberID == memberId && gm.GymID == manager.GymID);

            if (gymMember == null)
                return Forbid();

            return View(member);
        }

        // ================= Manage Subscriptions =================

        [HttpGet]
        public IActionResult Subscriptions()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            var subscriptions = _context.MemberGymSubs
                .Where(mgs => mgs.GymID == manager.GymID)
                .Include(mgs => mgs.GymSub)
                .Include(mgs => mgs.Member)
                .ThenInclude(m => m.User)
                .ToList();

            return View(subscriptions);
        }

        // ================= Reports =================

        [HttpGet]
        public IActionResult Reports()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var manager = _context.GymManagers
                .FirstOrDefault(m => m.UserID == userId);

            if (manager == null)
                return RedirectToAction("Dashboard");

            // Attendance Report
            var last30Days = DateTime.Now.AddDays(-30);

            var attendanceData = _context.Attendances
                .Where(a => a.CheckInTime >= last30Days)
                .GroupBy(a => a.CheckInTime.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Coach Performance
            var coachPerformance = _context.GymCoaches
                .Where(gc => gc.GymID == manager.GymID)
                .Include(gc => gc.Coach)
                .ThenInclude(c => c.Members)
                .Select(gc => new
                {
                    CoachName = gc.Coach.User.FirstName + " " + gc.Coach.User.LastName,
                    MemberCount = gc.Coach.Members.Count(),
                    Rating = gc.Coach.Rating
                })
                .ToList();

            ViewBag.AttendanceData = attendanceData;
            ViewBag.CoachPerformance = coachPerformance;

            return View();
        }
    }
}
