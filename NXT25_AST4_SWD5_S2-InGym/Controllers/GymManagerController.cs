using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
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

            var gymManager = _context.GymManagers
                .Include(gm => gm.Gym)
                .FirstOrDefault(gm => gm.UserID == userId);

            if (gymManager == null)
                return Unauthorized();

            var gym = gymManager.Gym;

            var stats = new
            {
                TotalCoaches = _context.GymCoaches
                    .Count(gc => gc.GymID == gym.GymID),
                TotalMembers = _context.GymMembers
                    .Count(gm => gm.GymID == gym.GymID),
                ActiveSubscriptions = _context.MemberGymSubs
                    .Count(mgs => mgs.IsActive),
                TotalAttendance = _context.Attendances
                    .Count()
            };

            ViewBag.Stats = stats;
            ViewBag.GymName = gym.GymName;
            ViewBag.GymID = gym.GymID;

            return View();
        }

        // ================= Manage Coaches =================

        public IActionResult Coaches()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var gymManager = _context.GymManagers
                .FirstOrDefault(gm => gm.UserID == userId);

            if (gymManager == null)
                return Unauthorized();

            var coaches = _context.GymCoaches
                .Where(gc => gc.GymID == gymManager.GymID)
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

        // ================= Manage Members =================

        public IActionResult Members()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var gymManager = _context.GymManagers
                .FirstOrDefault(gm => gm.UserID == userId);

            if (gymManager == null)
                return Unauthorized();

            var members = _context.GymMembers
                .Where(gm => gm.GymID == gymManager.GymID)
                .Include(gm => gm.Member)
                .ThenInclude(m => m.User)
                .Include(gm => gm.Member)
                .ThenInclude(m => m.Coach)
                .ThenInclude(c => c.User)
                .Select(gm => new
                {
                    MemberID = gm.Member.MemberID,
                    FullName = gm.Member.User.FirstName + " " + gm.Member.User.LastName,
                    Email = gm.Member.User.Email,
                    CoachName = gm.Member.Coach != null ? gm.Member.Coach.User.FirstName + " " + gm.Member.Coach.User.LastName : "Unassigned",
                    JoinDate = gm.Member.JoinDate,
                    IsActive = gm.Member.IsActive
                })
                .ToList();

            return View(members);
        }

        // ================= View Subscriptions =================

        public IActionResult Subscriptions()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var gymManager = _context.GymManagers
                .FirstOrDefault(gm => gm.UserID == userId);

            if (gymManager == null)
                return Unauthorized();

            var subscriptions = _context.MemberGymSubs
                .Include(mgs => mgs.Member)
                .ThenInclude(m => m.User)
                .Include(mgs => mgs.GymSub)
                .Where(mgs => mgs.Member.GymMembers.Any(gm => gm.GymID == gymManager.GymID))
                .ToList();

            return View(subscriptions);
        }

        // ================= View Attendance =================

        public IActionResult Attendance()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var gymManager = _context.GymManagers
                .FirstOrDefault(gm => gm.UserID == userId);

            if (gymManager == null)
                return Unauthorized();

            var attendance = _context.Attendances
                .Include(a => a.Member)
                .ThenInclude(m => m.User)
                .Include(a => a.Member)
                .ThenInclude(m => m.GymMembers)
                .Where(a => a.Member.GymMembers.Any(gm => gm.GymID == gymManager.GymID))
                .OrderByDescending(a => a.CheckInTime)
                .ToList();

            return View(attendance);
        }

        // ================= Member Details =================

        public IActionResult MemberDetails(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var gymManager = _context.GymManagers
                .FirstOrDefault(gm => gm.UserID == userId);

            if (gymManager == null)
                return Unauthorized();

            var member = _context.Members
                .Include(m => m.User)
                .Include(m => m.Coach)
                .ThenInclude(c => c.User)
                .Include(m => m.GymMembers)
                .FirstOrDefault(m => m.MemberID == id && m.GymMembers.Any(gm => gm.GymID == gymManager.GymID));

            if (member == null)
                return NotFound();

            ViewBag.MemberAttendanceCount = _context.Attendances
                .Count(a => a.MemberID == member.MemberID);

            ViewBag.MemberSubscriptionsCount = _context.MemberGymSubs
                .Count(mgs => mgs.MemberID == member.MemberID && mgs.IsActive);

            return View(member);
        }
    }
}
