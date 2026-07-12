using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        #endregion

        #region Get Session Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }
        #endregion

        #region Create Session
        public ActionResult Create()
        {
            LoadTrainersDropDowns();
            LoadCategoriesDropDowns();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(viewModel);
            }
            var Result = _sessionService.CreateSession(viewModel);
            if(Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session failed to Create";
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(viewModel);
            }
        }
        #endregion

        #region Edit Session
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainersDropDowns();
            return View(Session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id, UpdateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                return View(viewModel);
            }

            var Result = _sessionService.UpdateSession(id, viewModel);

            if(Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session failed to Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Session
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View(Session);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.DeleteSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session failed to Delete due to it is onGoing or not found";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Tag Helpers
        private void LoadTrainersDropDowns()
        {
            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadCategoriesDropDowns()
        {
            var Categories = _sessionService.GetAllCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        #endregion
    }
}
