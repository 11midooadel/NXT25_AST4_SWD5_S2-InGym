using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainers
        [HttpGet]
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        #endregion

        #region Create Trainer
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = _trainerService.CreateTrainer(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Trainer creation failed. Please try again.");
                return View(nameof(Create), model);
            }
        }
        #endregion

        #region Trainer Details
        public IActionResult Details(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        #endregion

        #region Edit Trainer
        public IActionResult Edit(int id)
        {
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit(UpdateTrainerViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _trainerService.UpdateTrainerDetails(model, id);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed to be Updated, check phone and mail";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        public IActionResult Delete(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View();
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _trainerService.RemoveTrainer(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to deleted trainer!";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
