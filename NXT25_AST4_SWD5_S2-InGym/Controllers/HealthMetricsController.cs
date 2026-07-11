using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    public class HealthMetricsController : Controller
    {
        private readonly GymDbContext _context;

        public HealthMetricsController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Coach: View Member Health Metrics =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult ViewMemberMetrics(int memberId)
        {
            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == memberId);

            if (member == null)
                return NotFound();

            var metrics = _context.HealthMetricLogs
                .Where(x => x.MemberID == memberId)
                .OrderByDescending(x => x.LogDate)
                .ToList();

            ViewBag.MemberID = memberId;
            ViewBag.MemberName = member.User.FirstName + " " + member.User.LastName;

            return View(metrics);
        }

        // ================= Coach: Log Health Metric =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult LogMetric(int memberId)
        {
            var member = _context.Members.FirstOrDefault(m => m.MemberID == memberId);

            if (member == null)
                return NotFound();

            var model = new HealthMetricLog
            {
                MemberID = memberId,
                LogDate = DateTime.Now
            };

            ViewBag.MemberID = memberId;
            return View(model);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogMetric(HealthMetricLog model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MemberID = model.MemberID;
                return View(model);
            }

            var metric = new HealthMetricLog
            {
                MemberID = model.MemberID,
                Weight = model.Weight,
                BodyFat = model.BodyFat,
                BMI = model.BMI,
                MuscleMass = model.MuscleMass,
                Notes = model.Notes,
                LogDate = DateTime.Now
            };

            _context.HealthMetricLogs.Add(metric);
            _context.SaveChanges();

            TempData["Success"] = "Health Metric Logged Successfully.";

            return RedirectToAction(nameof(ViewMemberMetrics), new { memberId = model.MemberID });
        }

        // ================= Member: View My Health Metrics =================

        [Authorize(Roles = "Member")]
        [HttpGet]
        public IActionResult MyMetrics()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var metrics = _context.HealthMetricLogs
                .Where(x => x.MemberID == member.MemberID)
                .OrderByDescending(x => x.LogDate)
                .ToList();

            return View(metrics);
        }

        // ================= Coach: Edit Health Metric =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult EditMetric(int id)
        {
            var metric = _context.HealthMetricLogs
                .FirstOrDefault(x => x.MetricID == id);

            if (metric == null)
                return NotFound();

            return View(metric);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMetric(int id, HealthMetricLog model)
        {
            var metric = _context.HealthMetricLogs
                .FirstOrDefault(x => x.MetricID == id);

            if (metric == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            metric.Weight = model.Weight;
            metric.BodyFat = model.BodyFat;
            metric.BMI = model.BMI;
            metric.MuscleMass = model.MuscleMass;
            metric.Notes = model.Notes;

            _context.SaveChanges();

            TempData["Success"] = "Health Metric Updated Successfully.";

            return RedirectToAction(nameof(ViewMemberMetrics), new { memberId = metric.MemberID });
        }

        // ================= Coach: Delete Health Metric =================

        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMetric(int id)
        {
            var metric = _context.HealthMetricLogs
                .FirstOrDefault(x => x.MetricID == id);

            if (metric == null)
                return NotFound();

            int memberId = metric.MemberID;

            _context.HealthMetricLogs.Remove(metric);
            _context.SaveChanges();

            TempData["Success"] = "Health Metric Deleted Successfully.";

            return RedirectToAction(nameof(ViewMemberMetrics), new { memberId });
        }
    }
}
