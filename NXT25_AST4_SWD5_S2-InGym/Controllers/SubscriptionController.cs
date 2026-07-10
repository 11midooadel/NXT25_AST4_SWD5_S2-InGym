using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Member")]
    public class SubscriptionController : Controller
    {
        private readonly GymDbContext _context;

        public SubscriptionController(GymDbContext context)
        {
            _context = context;
        }

        // عرض الباقات
        [HttpGet]
        public IActionResult ChooseSubscription()
        {
            var subscriptions = _context.GymSubs.ToList();

            return View(subscriptions);
        }

        // الاشتراك في باقة
        [HttpGet]
        public IActionResult Subscribe(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // الحصول على العضو
            var member = _context.Members
                                 .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
            {
                return RedirectToAction("CompleteProfile", "Member");
            }

            // الحصول على الجيم الذي انضم إليه
            var gymMember = _context.GymMembers
                                    .FirstOrDefault(g => g.MemberID == member.MemberID);

            if (gymMember == null)
            {
                return RedirectToAction("ChooseGym", "Member");
            }

            // منع الاشتراك مرتين
            bool alreadySubscribed = _context.MemberGymSubs
                                             .Any(x => x.MemberID == member.MemberID && x.IsActive);

            if (alreadySubscribed)
            {
                TempData["Error"] = "You already have an active subscription.";
                return RedirectToAction("Dashboard", "Home");
            }

            // الحصول على الباقة
            var gymSub = _context.GymSubs.FirstOrDefault(x => x.GymSubID == id);

            if (gymSub == null)
            {
                return NotFound();
            }

            MemberGymSub subscription = new MemberGymSub
            {
                MemberID = member.MemberID,
                GymID = gymMember.GymID,
                GymSubID = gymSub.GymSubID,

                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(gymSub.DurationMonths),
                IsActive = true
            };

            _context.MemberGymSubs.Add(subscription);
            _context.SaveChanges();

            TempData["Success"] = "Subscription completed successfully.";

            return RedirectToAction("Dashboard", "Home");
        }
    }
}