using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        #region Get All Plans
        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }
        #endregion

        #region Get Plan Details
        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanDetails(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        #endregion

        #region Update Plan 
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Cannot edit this plan because it has active memberships.";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id, UpdatePlanViewModel updatePlan)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatePlan);
            }
            var Result = _planService.UpdatePlan(id, updatePlan);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan is updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan failed to update";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Activate
        [HttpPost]
        public ActionResult Activate(int id)
        {
            var Result = _planService.ToggleStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Sttaus Changed";
            }
            else
            {
                TempData["ErrorMessage"] = "failed to change plan status";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
