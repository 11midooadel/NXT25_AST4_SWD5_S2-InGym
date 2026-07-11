using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels.Admin;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly GymDbContext _context;

        public AdminController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Dashboard =================

        public IActionResult Dashboard()
        {
            ViewBag.Gyms = _context.Gyms.Count();
            ViewBag.Members = _context.Members.Count();
            ViewBag.Coaches = _context.Coaches.Count();
            ViewBag.Managers = _context.GymManagers.Count();

            return View();
        }

        // ================= Add Gym =================

        [HttpGet]
        public IActionResult AddGym()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGym(AddGymViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Gym gym = new Gym
            {
                GymName = model.GymName,
                RegistrationDate = DateTime.Now
            };

            _context.Gyms.Add(gym);
            _context.SaveChanges();

            GymLocation location = new GymLocation
            {
                GymID = gym.GymID,
                Location = model.Location
            };

            _context.GymLocations.Add(location);
            _context.SaveChanges();

            TempData["Success"] = "Gym added successfully.";

            return RedirectToAction(nameof(Gyms));
        }

        // ================= View Gyms =================

        public IActionResult Gyms()
        {
            var gyms = _context.Gyms
                .Include(g => g.GymLocations)
                .Include(g => g.GymManagers)
                .ToList();

            return View(gyms);
        }

        // ================= Add Gym Manager =================

        [HttpGet]
        public IActionResult AddGymManager()
        {
            AddGymManagerViewModel model = new AddGymManagerViewModel();

            model.Gyms = _context.Gyms
                .Select(g => new SelectListItem
                {
                    Value = g.GymID.ToString(),
                    Text = g.GymName
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGymManager(AddGymManagerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Gyms = _context.Gyms
                    .Select(g => new SelectListItem
                    {
                        Value = g.GymID.ToString(),
                        Text = g.GymName
                    })
                    .ToList();

                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");

                model.Gyms = _context.Gyms
                    .Select(g => new SelectListItem
                    {
                        Value = g.GymID.ToString(),
                        Text = g.GymName
                    })
                    .ToList();

                return View(model);
            }

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                Role = "GymManager"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            GymManager manager = new GymManager
            {
                UserID = user.UserID,
                Salary = model.Salary,
                HireDate = model.HireDate,
                GymID = model.GymID
            };

            _context.GymManagers.Add(manager);
            _context.SaveChanges();

            TempData["Success"] = "Gym Manager added successfully.";

            return RedirectToAction(nameof(GymManagers));
        }

        // ================= View Gym Managers =================

        public IActionResult GymManagers()
        {
            var managers = _context.GymManagers
                .Include(x => x.User)
                .Include(x => x.Gym)
                .ToList();

            return View(managers);
        }
        // ================= Add Coach =================

        [HttpGet]
        public IActionResult AddCoach()
        {
            AddCoachViewModel model = new AddCoachViewModel();

            model.Gyms = _context.Gyms
                .Select(g => new SelectListItem
                {
                    Value = g.GymID.ToString(),
                    Text = g.GymName
                }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCoach(AddCoachViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Gyms = _context.Gyms
                    .Select(g => new SelectListItem
                    {
                        Value = g.GymID.ToString(),
                        Text = g.GymName
                    }).ToList();

                return View(model);
            }

            if (_context.Users.Any(x => x.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");

                model.Gyms = _context.Gyms
                    .Select(g => new SelectListItem
                    {
                        Value = g.GymID.ToString(),
                        Text = g.GymName
                    }).ToList();

                return View(model);
            }

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = model.Password,
                Role = "Coach"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            Coach coach = new Coach
            {
                UserID = user.UserID,
                Salary = model.Salary,
                HireDate = model.HireDate,
                Rating = 0,
                Speciality = model.Speciality
            };

            _context.Coaches.Add(coach);
            _context.SaveChanges();

            GymCoach gymCoach = new GymCoach
            {
                GymID = model.GymID,
                CoachID = coach.CoachID
            };

            _context.GymCoaches.Add(gymCoach);
            _context.SaveChanges();

            TempData["Success"] = "Coach added successfully.";

            return RedirectToAction(nameof(Coaches));
        }

        // ================= View Coaches =================

        public IActionResult Coaches()
        {
            var coaches = _context.GymCoaches
                .Include(gc => gc.Coach)
                    .ThenInclude(c => c.User)
                .Include(gc => gc.Gym)
                .ToList();

            return View(coaches);
        }

        // ================= View Members =================

        public IActionResult Members()
        {
            var members = _context.Members
                .Include(m => m.User)
                .Include(m => m.Coach)
                .ThenInclude(c => c.User)
                .ToList();

            return View(members);
        }

        // ================= View Member Details =================

        [HttpGet]
        public IActionResult MemberDetails(int id)
        {
            var member = _context.Members
                .Include(m => m.User)
                .Include(m => m.Coach)
                .ThenInclude(c => c.User)
                .FirstOrDefault(m => m.MemberID == id);

            if (member == null)
                return NotFound();

            return View(member);
        }

        // ================= Edit Member =================

        [HttpGet]
        public IActionResult EditMember(int id)
        {
            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == id);

            if (member == null)
                return NotFound();

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMember(int id, Member model)
        {
            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == id);

            if (member == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            member.BirthDate = model.BirthDate;
            member.Gender = model.Gender;
            member.Height = model.Height;
            member.Weight = model.Weight;
            member.Goals = model.Goals;
            member.PhysRestrictions = model.PhysRestrictions;
            member.ChronicDiseases = model.ChronicDiseases;
            member.IsActive = model.IsActive;

            _context.SaveChanges();

            TempData["Success"] = "Member updated successfully.";

            return RedirectToAction(nameof(Members));
        }

        // ================= Deactivate Member =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeactivateMember(int id)
        {
            var member = _context.Members.FirstOrDefault(m => m.MemberID == id);

            if (member == null)
                return NotFound();

            member.IsActive = false;
            _context.SaveChanges();

            TempData["Success"] = "Member deactivated successfully.";

            return RedirectToAction(nameof(Members));
        }

        // ================= Edit Gym =================

        [HttpGet]
        public IActionResult EditGym(int id)
        {
            var gym = _context.Gyms
                .Include(g => g.GymLocations)
                .FirstOrDefault(g => g.GymID == id);

            if (gym == null)
                return NotFound();

            return View(gym);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditGym(int id, Gym model)
        {
            var gym = _context.Gyms
                .Include(g => g.GymLocations)
                .FirstOrDefault(g => g.GymID == id);

            if (gym == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            gym.GymName = model.GymName;

            _context.SaveChanges();

            TempData["Success"] = "Gym updated successfully.";

            return RedirectToAction(nameof(Gyms));
        }

        // ================= Delete Gym =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGym(int id)
        {
            var gym = _context.Gyms.FirstOrDefault(g => g.GymID == id);

            if (gym == null)
                return NotFound();

            // Delete all related data
            var locations = _context.GymLocations.Where(gl => gl.GymID == id);
            _context.GymLocations.RemoveRange(locations);

            var managers = _context.GymManagers.Where(gm => gm.GymID == id);
            _context.GymManagers.RemoveRange(managers);

            var gymCoaches = _context.GymCoaches.Where(gc => gc.GymID == id);
            _context.GymCoaches.RemoveRange(gymCoaches);

            var gymMembers = _context.GymMembers.Where(gm => gm.GymID == id);
            _context.GymMembers.RemoveRange(gymMembers);

            var subscriptions = _context.MemberGymSubs.Where(mgs => mgs.GymID == id);
            _context.MemberGymSubs.RemoveRange(subscriptions);

            _context.Gyms.Remove(gym);
            _context.SaveChanges();

            TempData["Success"] = "Gym deleted successfully.";

            return RedirectToAction(nameof(Gyms));
        }

        // ================= Manage Subscriptions =================

        public IActionResult Subscriptions()
        {
            var gymSubs = _context.GymSubs.ToList();
            var privateSubs = _context.PrivateSubs.ToList();

            ViewBag.GymSubs = gymSubs;
            ViewBag.PrivateSubs = privateSubs;

            return View();
        }

        // ================= Add Gym Subscription =================

        [HttpGet]
        public IActionResult AddGymSubscription()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGymSubscription(GymSub model)
        {
            if (!ModelState.IsValid)
                return View(model);

            GymSub sub = new GymSub
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                DurationMonths = model.DurationMonths
            };

            _context.GymSubs.Add(sub);
            _context.SaveChanges();

            TempData["Success"] = "Gym Subscription added successfully.";

            return RedirectToAction(nameof(Subscriptions));
        }

        // ================= Edit Gym Subscription =================

        [HttpGet]
        public IActionResult EditGymSubscription(int id)
        {
            var sub = _context.GymSubs.FirstOrDefault(g => g.GymSubID == id);

            if (sub == null)
                return NotFound();

            return View(sub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditGymSubscription(int id, GymSub model)
        {
            var sub = _context.GymSubs.FirstOrDefault(g => g.GymSubID == id);

            if (sub == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            sub.Name = model.Name;
            sub.Description = model.Description;
            sub.Price = model.Price;
            sub.DurationMonths = model.DurationMonths;

            _context.SaveChanges();

            TempData["Success"] = "Gym Subscription updated successfully.";

            return RedirectToAction(nameof(Subscriptions));
        }

        // ================= Delete Gym Subscription =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGymSubscription(int id)
        {
            var sub = _context.GymSubs.FirstOrDefault(g => g.GymSubID == id);

            if (sub == null)
                return NotFound();

            _context.GymSubs.Remove(sub);
            _context.SaveChanges();

            TempData["Success"] = "Gym Subscription deleted successfully.";

            return RedirectToAction(nameof(Subscriptions));
        }

        // ================= Add Private Subscription =================

        [HttpGet]
        public IActionResult AddPrivateSubscription()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPrivateSubscription(PrivateSub model)
        {
            if (!ModelState.IsValid)
                return View(model);

            PrivateSub sub = new PrivateSub
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                DurationMonths = model.DurationMonths
            };

            _context.PrivateSubs.Add(sub);
            _context.SaveChanges();

            TempData["Success"] = "Private Subscription added successfully.";

            return RedirectToAction(nameof(Subscriptions));
        }

        // ================= Edit Private Subscription =================

        [HttpGet]
        public IActionResult EditPrivateSubscription(int id)
        {
            var sub = _context.PrivateSubs.FirstOrDefault(p => p.PrivateSubID == id);

            if (sub == null)
                return NotFound();

            return View(sub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPrivateSubscription(int id, PrivateSub model)
        {
            var sub = _context.PrivateSubs.FirstOrDefault(p => p.PrivateSubID == id);

            if (sub == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            sub.Name = model.Name;
            sub.Description = model.Description;
            sub.Price = model.Price;
            sub.DurationMonths = model.DurationMonths;

            _context.SaveChanges();

            TempData["Success"] = "Private Subscription updated successfully.";

            return RedirectToAction(nameof(Subscriptions));
        }

        // ================= Delete Private Subscription =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePrivateSubscription(int id)
        {
            var sub = _context.PrivateSubs.FirstOrDefault(p => p.PrivateSubID == id);

            if (sub == null)
                return NotFound();

            _context.PrivateSubs.Remove(sub);
            _context.SaveChanges();

            TempData["Success"] = "Private Subscription deleted successfully.";

            return RedirectToAction(nameof(Subscriptions));
        }

        // ================= Manage Exercises =================

        public IActionResult Exercises()
        {
            var exercises = _context.Exercises.ToList();

            return View(exercises);
        }

        // ================= Add Exercise =================

        [HttpGet]
        public IActionResult AddExercise()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExercise(Exercise model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Exercise exercise = new Exercise
            {
                ExerciseName = model.ExerciseName,
                Description = model.Description,
                MuscleGroup = model.MuscleGroup
            };

            _context.Exercises.Add(exercise);
            _context.SaveChanges();

            TempData["Success"] = "Exercise added successfully.";

            return RedirectToAction(nameof(Exercises));
        }

        // ================= Edit Exercise =================

        [HttpGet]
        public IActionResult EditExercise(int id)
        {
            var exercise = _context.Exercises.FirstOrDefault(e => e.ExerciseID == id);

            if (exercise == null)
                return NotFound();

            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditExercise(int id, Exercise model)
        {
            var exercise = _context.Exercises.FirstOrDefault(e => e.ExerciseID == id);

            if (exercise == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            exercise.ExerciseName = model.ExerciseName;
            exercise.Description = model.Description;
            exercise.MuscleGroup = model.MuscleGroup;

            _context.SaveChanges();

            TempData["Success"] = "Exercise updated successfully.";

            return RedirectToAction(nameof(Exercises));
        }

        // ================= Delete Exercise =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExercise(int id)
        {
            var exercise = _context.Exercises.FirstOrDefault(e => e.ExerciseID == id);

            if (exercise == null)
                return NotFound();

            _context.Exercises.Remove(exercise);
            _context.SaveChanges();

            TempData["Success"] = "Exercise deleted successfully.";

            return RedirectToAction(nameof(Exercises));
        }
    }
}