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

            // لو العضو كمل البروفايل قبل كده
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
        public IActionResult JoinGym(int id)
        {
            // الحصول على UserID من الـ Cookie
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // الحصول على الـ Member الحالي
            Member? member = _context.Members
                                     .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
            {
                return RedirectToAction("CompleteProfile");
            }

            // التأكد أنه غير منضم لنفس الجيم
            bool alreadyJoined = _context.GymMembers
                                         .Any(gm => gm.MemberID == member.MemberID &&
                                                    gm.GymID == id);

            if (alreadyJoined)
            {
                TempData["Error"] = "You are already a member of this gym.";
                return RedirectToAction("ChooseGym");
            }

            GymMember gymMember = new GymMember
            {
                MemberID = member.MemberID,
                GymID = id
            };

            _context.GymMembers.Add(gymMember);
            _context.SaveChanges();

            TempData["Success"] = "Gym joined successfully.";

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public IActionResult ChooseGym()
        {
            var gyms = _context.Gyms
                               .Include(g => g.GymLocations)
                               .ToList();

            return View(gyms);
        }
    }
}