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
            {
                return RedirectToAction("CompleteProfile", "Member");
            }

            // الجيم الذي انضم إليه العضو
            var gymMember = _context.GymMembers
                                    .Include(gm => gm.Gym)
                                    .ThenInclude(g => g.GymLocations)
                                    .FirstOrDefault(gm => gm.MemberID == member.MemberID);

            DashboardViewModel model = new DashboardViewModel
            {
                FullName = member.User.FirstName + " " + member.User.LastName,
                Email = member.User.Email,
                Gender = member.Gender,
                Height = member.Height,
                Weight = member.Weight,
                Goals = member.Goals,
                JoinDate = member.JoinDate,
                IsActive = member.IsActive,

                GymName = gymMember?.Gym.GymName ?? "No Gym Selected",
                GymLocation = gymMember?.Gym.GymLocations.FirstOrDefault()?.Location ?? "No Location"
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}