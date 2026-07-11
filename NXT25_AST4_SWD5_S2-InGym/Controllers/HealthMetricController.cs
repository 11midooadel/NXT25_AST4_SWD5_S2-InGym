using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize]
    public class HealthMetricController : Controller
    {
        private readonly GymDbContext _context;

        public HealthMetricController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Coach : Log Health Metric =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult LogMetric(int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .FirstOrDefault(m => m.MemberID == memberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound("Member not found or not assigned to this coach.");

            var viewModel = new LogHealthMetricViewModel
            {
                MemberId = memberId,
                MemberName = member.User?.FirstName + " " + member.User?.LastName ?? "Unknown"
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogMetric(LogHealthMetricViewModel model)
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

            // Create health metric log
            var healthMetricLog = new HealthMetricLog
            {
                Weight = model.Weight,
                BodyFatPercentage = model.BodyFatPercentage,
                LogDate = DateTime.Now
            };

            _context.HealthMetricLogs.Add(healthMetricLog);
            _context.SaveChanges();

            // Create member health metric relationship
            var memberHealthMetric = new MemberHealthMetric
            {
                MemberID = model.MemberId,
                MetricID = healthMetricLog.MetricID
            };

            _context.MemberHealthMetrics.Add(memberHealthMetric);
            _context.SaveChanges();

            TempData["Success"] = "Health metric logged successfully.";
            return RedirectToAction("ManageMember", "Coach", new { id = model.MemberId });
        }

        // ================= Coach : View Member Health Metrics =================

        [Authorize(Roles = "Coach")]
        public IActionResult ViewMetrics(int memberId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return Unauthorized();

            var member = _context.Members
                .Include(m => m.User)
                .Include(m => m.MemberHealthMetrics)
                .ThenInclude(mhm => mhm.HealthMetricLog)
                .FirstOrDefault(m => m.MemberID == memberId && m.CoachID == coach.CoachID);

            if (member == null)
                return NotFound();

            var metrics = member.MemberHealthMetrics
                .OrderByDescending(mhm => mhm.HealthMetricLog.LogDate)
                .Select(mhm => new HealthMetricViewModel
                {
                    MetricID = mhm.MetricID,
                    Weight = mhm.HealthMetricLog.Weight,
                    BodyFatPercentage = mhm.HealthMetricLog.BodyFatPercentage,
                    LogDate = mhm.HealthMetricLog.LogDate,
                    MemberName = member.User.FirstName + " " + member.User.LastName
                })
                .ToList();

            return View(metrics);
        }

        // ================= Member : View Own Health Metrics =================

        [Authorize(Roles = "Member")]
        public IActionResult MyMetrics()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .Include(m => m.User)
                .Include(m => m.MemberHealthMetrics)
                .ThenInclude(mhm => mhm.HealthMetricLog)
                .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var metrics = member.MemberHealthMetrics
                .OrderByDescending(mhm => mhm.HealthMetricLog.LogDate)
                .Select(mhm => new HealthMetricViewModel
                {
                    MetricID = mhm.MetricID,
                    Weight = mhm.HealthMetricLog.Weight,
                    BodyFatPercentage = mhm.HealthMetricLog.BodyFatPercentage,
                    LogDate = mhm.HealthMetricLog.LogDate,
                    MemberName = member.User.FirstName + " " + member.User.LastName
                })
                .ToList();

            return View(metrics);
        }

        // ================= Coach : Delete Health Metric =================

        [Authorize(Roles = "Coach")]
        [HttpPost]
        public IActionResult DeleteMetric(int metricId, int memberId)
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

            var memberHealthMetric = _context.MemberHealthMetrics
                .FirstOrDefault(mhm => mhm.MetricID == metricId && mhm.MemberID == memberId);

            if (memberHealthMetric == null)
                return NotFound();

            var healthMetricLog = _context.HealthMetricLogs
                .FirstOrDefault(hml => hml.MetricID == metricId);

            if (healthMetricLog != null)
            {
                _context.HealthMetricLogs.Remove(healthMetricLog);
            }

            _context.MemberHealthMetrics.Remove(memberHealthMetric);
            _context.SaveChanges();

            TempData["Success"] = "Health metric deleted successfully.";
            return RedirectToAction("ViewMetrics", new { memberId });
        }
    }
}
