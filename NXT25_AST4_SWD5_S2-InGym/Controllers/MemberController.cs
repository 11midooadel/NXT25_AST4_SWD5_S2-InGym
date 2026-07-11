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
    public class MemberController : Controller
    {
        private readonly GymDbContext _context;

        public MemberController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Complete Profile =================

        [HttpGet]
        public IActionResult CompleteProfile()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (_context.Members.Any(m => m.UserID == userId))
            {
                return RedirectToAction("ChooseGym");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteProfile(MemberProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (_context.Members.Any(m => m.UserID == userId))
            {
                TempData["Error"] = "Profile already completed.";
                return RedirectToAction("ChooseGym");
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

            return RedirectToAction("ChooseGym");
        }

        // ================= Choose Gym =================

        [HttpGet]
        public IActionResult ChooseGym()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            Member? member = _context.Members
                                     .FirstOrDefault(m => m.UserID == userId);

            if (member != null)
            {
                bool joined = _context.GymMembers
                                      .Any(g => g.MemberID == member.MemberID);

                if (joined)
                {
                    return RedirectToAction("ChooseSubscription", "Subscription");
                }
            }

            var gyms = _context.Gyms
                               .Include(g => g.GymLocations)
                               .ToList();

            return View(gyms);
        }

        // ================= Join Gym =================

        [HttpGet]
        public IActionResult JoinGym(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            Member? member = _context.Members
                                     .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
            {
                return RedirectToAction("CompleteProfile");
            }

            bool alreadyJoined = _context.GymMembers
                                         .Any(g => g.MemberID == member.MemberID);

            if (!alreadyJoined)
            {
                GymMember gymMember = new GymMember
                {
                    MemberID = member.MemberID,
                    GymID = id
                };

                _context.GymMembers.Add(gymMember);
                _context.SaveChanges();

                TempData["Success"] = "Gym joined successfully.";
            }

            return RedirectToAction("ChooseSubscription", "Subscription");
        }
    }
}