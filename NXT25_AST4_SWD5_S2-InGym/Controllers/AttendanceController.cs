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
    public class AttendanceController : Controller
    {
        private readonly GymDbContext _context;

        public AttendanceController(GymDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Attendance Page
        // ==========================
        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .FirstOrDefault(x => x.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            DateTime today = DateTime.Today;

            var todayAttendance = _context.Attendances
                .FirstOrDefault(x =>
                    x.MemberID == member.MemberID &&
                    x.CheckInTime.Date == today);

            AttendanceViewModel model = new AttendanceViewModel
            {
                CheckedIn = todayAttendance != null,

                CheckInTime = todayAttendance?.CheckInTime,

                CheckOutTime = todayAttendance?.CheckOutTime,

                AttendanceHistory = _context.Attendances
                    .Where(x => x.MemberID == member.MemberID)
                    .OrderByDescending(x => x.CheckInTime)
                    .ToList()
            };

            return View(model);
        }

        // ==========================
        // Check In
        // ==========================
        [HttpGet]
        public IActionResult CheckIn()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .FirstOrDefault(x => x.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            bool alreadyChecked =
                _context.Attendances.Any(x =>
                    x.MemberID == member.MemberID &&
                    x.CheckInTime.Date == DateTime.Today);

            if (alreadyChecked)
            {
                TempData["Error"] = "You already checked in today.";

                return RedirectToAction(nameof(Index));
            }

            Attendance attendance = new Attendance
            {
                MemberID = member.MemberID,

                CheckInTime = DateTime.Now
            };

            _context.Attendances.Add(attendance);

            _context.SaveChanges();

            TempData["Success"] = "Check In Successful.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Check Out
        // ==========================
        [HttpGet]
        public IActionResult CheckOut()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .FirstOrDefault(x => x.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var attendance = _context.Attendances
                .FirstOrDefault(x =>
                    x.MemberID == member.MemberID &&
                    x.CheckInTime.Date == DateTime.Today);

            if (attendance == null)
            {
                TempData["Error"] = "You must Check In first.";

                return RedirectToAction(nameof(Index));
            }

            if (attendance.CheckOutTime != null)
            {
                TempData["Error"] = "You already checked out.";

                return RedirectToAction(nameof(Index));
            }

            attendance.CheckOutTime = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Check Out Successful.";

            return RedirectToAction(nameof(Index));
        }
    }
}