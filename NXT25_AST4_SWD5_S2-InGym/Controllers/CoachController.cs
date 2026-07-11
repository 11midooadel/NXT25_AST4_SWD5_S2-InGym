using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NXT25_AST4_SWD5_S2_InGym.Data;
using NXT25_AST4_SWD5_S2_InGym.Models;
using NXT25_AST4_SWD5_S2_InGym.ViewModels;
using NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach;
using System.Security.Claims;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    [Authorize]
    public class CoachController : Controller
    {
        private readonly GymDbContext _context;

        public CoachController(GymDbContext context)
        {
            _context = context;
        }

        // ================= Coach Dashboard =================

        [Authorize(Roles = "Coach")]
        public IActionResult Dashboard()
        {
            return View();
        }

        // ================= Member : Choose Gym Coach =================

        [Authorize(Roles = "Member")]
        [HttpGet]
        public IActionResult ChooseGymCoach()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var subscription = _context.MemberGymSubs
                .FirstOrDefault(x => x.MemberID == member.MemberID && x.IsActive);

            if (subscription == null)
                return RedirectToAction("ChooseSubscription", "Subscription");

            var coaches = _context.GymCoaches
                .Where(gc => gc.GymID == subscription.GymID)
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

        // ================= Member : Select Gym Coach =================

        [Authorize(Roles = "Member")]
        [HttpGet]
        public IActionResult SelectCoach(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var member = _context.Members
                .FirstOrDefault(m => m.UserID == userId);

            if (member == null)
                return RedirectToAction("CompleteProfile", "Member");

            var coach = _context.Coaches
                .FirstOrDefault(c => c.CoachID == id);

            if (coach == null)
                return NotFound();

            member.CoachID = coach.CoachID;

            _context.SaveChanges();

            TempData["Success"] = "Gym coach selected successfully.";

            return RedirectToAction("Dashboard", "Home");
        }
        // ================= Coach : My Members =================

        [Authorize(Roles = "Coach")]
        public IActionResult Members()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return RedirectToAction("Dashboard");

            var members = _context.Members
                .Where(m => m.CoachID == coach.CoachID)
                .Include(m => m.User)
                .ToList();

            return View(members);
        }
        public IActionResult ManageMember(int id)
        {
            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.MemberID == id);

            if (member == null)
                return NotFound();

            return View(member);
        }
        [Authorize(Roles = "Coach")]
        public IActionResult WorkoutPlans(int? memberId = null)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return RedirectToAction("Dashboard");

            if (memberId.HasValue)
            {
                // Show plans for specific member
                ViewBag.MemberID = memberId;
                var plans = _context.MemberWorkoutPlans
                    .Where(x => x.MemberID == memberId)
                    .Include(x => x.WorkoutPlan)
                    .Select(x => x.WorkoutPlan)
                    .ToList();

                return View(plans);
            }
            else
            {
                // Show all members and their workout plans
                var allMembers = _context.Members
                    .Where(m => m.CoachID == coach.CoachID)
                    .Include(m => m.User)
                    .ToList();

                return View("WorkoutPlansOverview", allMembers);
            }
        }
        // ================= Coach : Diet Plans Overview =================

        [Authorize(Roles = "Coach")]
        public IActionResult DietPlans()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var coach = _context.Coaches
                .FirstOrDefault(c => c.UserID == userId);

            if (coach == null)
                return RedirectToAction("Dashboard");

            // Show all members so coach can select one to create/view diet plans
            var allMembers = _context.Members
                .Where(m => m.CoachID == coach.CoachID)
                .Include(m => m.User)
                .ToList();

            return View("DietPlansOverview", allMembers);
        }
        // ================= Create Workout Plan =================

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult CreateWorkoutPlan(int memberId)
        {
            var model = new CreateWorkoutPlanViewModel
            {
                MemberID = memberId,

                Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName + " (" + x.MuscleGroup + ")"
                    })
                    .ToList()
            };

            return View(model);
        }
        [Authorize(Roles = "Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWorkoutPlan(CreateWorkoutPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName + " (" + x.MuscleGroup + ")"
                    })
                    .ToList();

                return View(model);
            }
            return Content(
    $"MemberID = {model.MemberID}\n" +
    $"Goal = {model.Goal}\n" +
    $"Exercises Count = {model.SelectedExercises.Count}"
);

            WorkoutPlan plan = new WorkoutPlan
            {
                Goal = model.Goal,
                CreationDate = DateTime.Now,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                IsActive = true
            };

            _context.WorkoutPlans.Add(plan);
            _context.SaveChanges();

            MemberWorkoutPlan memberPlan = new MemberWorkoutPlan
            {
                MemberID = model.MemberID,
                PlanID = plan.PlanID
            };

            _context.MemberWorkoutPlans.Add(memberPlan);

            foreach (var exerciseId in model.SelectedExercises)
            {
                WorkoutPlanExercise item = new WorkoutPlanExercise
                {
                    PlanID = plan.PlanID,
                    ExerciseID = exerciseId,

                    WorkoutDay = DayOfWeek.Monday,

                    Sets = 3,
                    Reps = 12
                };

                _context.WorkoutPlanExercises.Add(item);
            }

            _context.SaveChanges();

            TempData["Success"] = "Workout Plan Created Successfully.";

            return RedirectToAction(nameof(WorkoutPlans), new
            {
                memberId = model.MemberID
            });
        }
        // ================= Manage Workout Plan =================

        [Authorize(Roles = "Coach")]
        public IActionResult ManageWorkoutPlan(int id)
        {
            var plan = _context.WorkoutPlans
                .Include(x => x.WorkoutPlanExercises)
                .ThenInclude(x => x.Exercise)
                .FirstOrDefault(x => x.PlanID == id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }
        // ================= Add Exercise To Plan =================

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult AddExercise(int planId)
        {
            AddExerciseToPlanViewModel model = new()
            {
                PlanID = planId,

                Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName + " (" + x.MuscleGroup + ")"
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult AddExercise(AddExerciseToPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName + " (" + x.MuscleGroup + ")"
                    }).ToList();

                return View(model);
            }

            WorkoutPlanExercise exercise = new()
            {
                PlanID = model.PlanID,
                ExerciseID = model.ExerciseID,
                WorkoutDay = model.WorkoutDay,
                Sets = model.Sets,
                Reps = model.Reps
            };

            _context.WorkoutPlanExercises.Add(exercise);

            _context.SaveChanges();

            TempData["Success"] = "Exercise Added Successfully.";

            return RedirectToAction(nameof(ManageWorkoutPlan), new
            {
                id = model.PlanID
            });
        }
        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult EditExercise(int planId, int exerciseId)
        {
            var item = _context.WorkoutPlanExercises
                .FirstOrDefault(x => x.PlanID == planId &&
                                     x.ExerciseID == exerciseId);

            if (item == null)
                return NotFound();

            EditExerciseViewModel model = new()
            {
                PlanID = item.PlanID,
                ExerciseID = item.ExerciseID,
                WorkoutDay = item.WorkoutDay,
                Sets = item.Sets,
                Reps = item.Reps,

                Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName
                    }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coach")]
        public IActionResult EditExercise(EditExerciseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName
                    }).ToList();

                return View(model);
            }

            var item = _context.WorkoutPlanExercises
                .FirstOrDefault(x => x.PlanID == model.PlanID &&
                                     x.ExerciseID == model.ExerciseID);

            if (item == null)
                return NotFound();

            item.WorkoutDay = model.WorkoutDay;
            item.Sets = model.Sets;
            item.Reps = model.Reps;

            _context.SaveChanges();

            TempData["Success"] = "Exercise Updated Successfully.";

            return RedirectToAction(nameof(ManageWorkoutPlan),
                new { id = model.PlanID });
        }
        [Authorize(Roles = "Coach")]
        public IActionResult DeleteExercise(int planId, int exerciseId)
        {
            var item = _context.WorkoutPlanExercises
                .FirstOrDefault(x => x.PlanID == planId &&
                                     x.ExerciseID == exerciseId);

            if (item == null)
                return NotFound();

            _context.WorkoutPlanExercises.Remove(item);

            _context.SaveChanges();

            TempData["Success"] = "Exercise Deleted Successfully.";

            return RedirectToAction(nameof(ManageWorkoutPlan),
                new { id = planId });
        }

        // ================= Edit Exercise In Plan =================

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult EditExercise(int planId, int exerciseId)
        {
            var exercise = _context.WorkoutPlanExercises
                .Include(x => x.Exercise)
                .FirstOrDefault(x => x.PlanID == planId && x.ExerciseID == exerciseId);

            if (exercise == null)
                return NotFound();

            var model = new AddExerciseToPlanViewModel
            {
                PlanID = planId,
                ExerciseID = exerciseId,
                WorkoutDay = exercise.WorkoutDay,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName + " (" + x.MuscleGroup + ")"
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult EditExercise(int planId, int exerciseId, AddExerciseToPlanViewModel model)
        {
            var exercise = _context.WorkoutPlanExercises
                .FirstOrDefault(x => x.PlanID == planId && x.ExerciseID == exerciseId);

            if (exercise == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Exercises = _context.Exercises
                    .Select(x => new SelectListItem
                    {
                        Value = x.ExerciseID.ToString(),
                        Text = x.ExerciseName + " (" + x.MuscleGroup + ")"
                    }).ToList();

                return View(model);
            }

            exercise.WorkoutDay = model.WorkoutDay;
            exercise.Sets = model.Sets;
            exercise.Reps = model.Reps;

            _context.SaveChanges();

            TempData["Success"] = "Exercise Updated Successfully.";

            return RedirectToAction(nameof(ManageWorkoutPlan), new
            {
                id = model.PlanID
            });
        }

        // ================= Delete Exercise From Plan =================

        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExercise(int planId, int exerciseId)
        {
            var exercise = _context.WorkoutPlanExercises
                .FirstOrDefault(x => x.PlanID == planId && x.ExerciseID == exerciseId);

            if (exercise == null)
                return NotFound();

            _context.WorkoutPlanExercises.Remove(exercise);
            _context.SaveChanges();

            TempData["Success"] = "Exercise Removed Successfully.";

            return RedirectToAction(nameof(ManageWorkoutPlan), new
            {
                id = planId
            });
        }

        // ================= Edit Workout Plan =================

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult EditWorkoutPlan(int id)
        {
            var plan = _context.WorkoutPlans
                .FirstOrDefault(x => x.PlanID == id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult EditWorkoutPlan(int id, WorkoutPlan model)
        {
            var plan = _context.WorkoutPlans
                .FirstOrDefault(x => x.PlanID == id);

            if (plan == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            plan.Goal = model.Goal;
            plan.StartDate = model.StartDate;
            plan.EndDate = model.EndDate;
            plan.IsActive = model.IsActive;

            _context.SaveChanges();

            TempData["Success"] = "Workout Plan Updated Successfully.";

            return RedirectToAction(nameof(ManageWorkoutPlan), new
            {
                id = id
            });
        }

        // ================= Delete Workout Plan =================

        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteWorkoutPlan(int id)
        {
            var plan = _context.WorkoutPlans.FirstOrDefault(x => x.PlanID == id);

            if (plan == null)
                return NotFound();

            var memberPlan = _context.MemberWorkoutPlans
                .FirstOrDefault(x => x.PlanID == id);

            int memberId = memberPlan?.MemberID ?? 0;

            // Delete exercises
            var exercises = _context.WorkoutPlanExercises.Where(x => x.PlanID == id);
            _context.WorkoutPlanExercises.RemoveRange(exercises);

            // Delete member plan
            _context.MemberWorkoutPlans.RemoveRange(
                _context.MemberWorkoutPlans.Where(x => x.PlanID == id)
            );

            // Delete plan
            _context.WorkoutPlans.Remove(plan);
            _context.SaveChanges();

            TempData["Success"] = "Workout Plan Deleted Successfully.";

            return RedirectToAction(nameof(WorkoutPlans), new
            {
                memberId = memberId
            });
        }
    }
}