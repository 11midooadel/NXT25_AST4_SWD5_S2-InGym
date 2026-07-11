using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    public class HomeController : Controller
    {
        private readonly GymDbContext _context;

        public HomeController(GymDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                                 .Include(m => m.User)
                                 .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            // Gym
            var gymMember = _context.GymMembers
                                    .Include(g => g.Gym)
                                    .ThenInclude(g => g.GymLocations)
                                    .FirstOrDefault(x => x.MemberID == member.MemberID);

            // Gym Subscription
            var gymSubscription = _context.MemberGymSubs
                                          .Include(x => x.GymSub)
                                          .FirstOrDefault(x => x.MemberID == member.MemberID &&
                                                               x.IsActive);

            // Gym Coach
            var coach = _context.Coaches
                                .Include(c => c.User)
                                .FirstOrDefault(c => c.CoachID == member.CoachID);

            // Private Trainer
            var privateSubscription = _context.MemberPrivateSubs
                                              .Include(x => x.Coach)
                                              .ThenInclude(c => c.User)
                                              .Include(x => x.PrivateSub)
                                              .FirstOrDefault(x => x.MemberID == member.MemberID &&
                                                                   x.IsActive);

            // Today's Attendance
            var todayAttendance = _context.Attendances
                                          .FirstOrDefault(a =>
                                              a.MemberID == member.MemberID &&
                                              a.CheckInTime.Date == DateTime.Today);

            DashboardViewModel model = new DashboardViewModel
            {
                // ================= Member =================
                FullName = member.User.FirstName + " " + member.User.LastName,
                Email = member.User.Email,
                Gender = member.Gender,
                Height = member.Height,
                Weight = member.Weight,
                Goals = member.Goals,
                JoinDate = member.JoinDate,
                IsActive = member.IsActive,

                // ================= Gym =================
                GymName = gymMember?.Gym.GymName ?? "No Gym",
                GymLocation = gymMember?.Gym.GymLocations.FirstOrDefault()?.Location ?? "-",

                // ================= Gym Subscription =================
                SubscriptionPrice = gymSubscription?.GymSub.Price ?? 0,
                DurationMonths = gymSubscription?.GymSub.DurationMonths ?? 0,
                StartDate = gymSubscription?.StartDate ?? DateTime.MinValue,
                EndDate = gymSubscription?.EndDate ?? DateTime.MinValue,
                SubscriptionActive = gymSubscription?.IsActive ?? false,
                DaysRemaining = gymSubscription == null
                    ? 0
                    : Math.Max((gymSubscription.EndDate - DateTime.Now).Days, 0),

                // ================= Gym Coach =================
                CoachName = coach == null
                    ? "No Coach Selected"
                    : coach.User.FirstName + " " + coach.User.LastName,

                CoachEmail = coach?.User.Email ?? "-",
                CoachSpeciality = coach?.Speciality ?? "-",
                CoachRating = coach?.Rating ?? 0,

                // ================= Private Trainer =================
                HasPrivateSubscription = privateSubscription != null,

                PrivateCoachName = privateSubscription == null
                    ? ""
                    : privateSubscription.Coach.User.FirstName + " " + privateSubscription.Coach.User.LastName,

                PrivateCoachEmail = privateSubscription?.Coach.User.Email ?? "",
                PrivateCoachSpeciality = privateSubscription?.Coach.Speciality ?? "",
                PrivateCoachRating = privateSubscription?.Coach.Rating ?? 0,

                PrivatePackagePrice = privateSubscription?.PrivateSub.Price ?? 0,
                PrivateSessionCount = privateSubscription?.PrivateSub.SessionCount ?? 0,
                PrivateEndDate = privateSubscription?.EndDate ?? DateTime.MinValue,

                // ================= Attendance =================
                CheckedInToday = todayAttendance != null,
                TodayCheckIn = todayAttendance?.CheckInTime,
                TodayCheckOut = todayAttendance?.CheckOutTime
            };

            return View(model);
        }
    }
}