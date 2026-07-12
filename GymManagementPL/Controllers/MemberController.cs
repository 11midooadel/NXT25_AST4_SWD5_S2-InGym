using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion

        #region GetMember Data
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or negatice Number";
                return RedirectToAction(nameof(Index));
            }


            var Member = _memberService.GetMemberDetails(id);

            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        #endregion

        #region Get Health Record
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of HealthRecord can not be 0 or negatice Number";
                return RedirectToAction(nameof(Index));
            }

            var HealthRecord = _memberService.GetMemberHealthRecord(id);

            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(HealthRecord);
        }
        #endregion

        #region Add Member
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "check Data and Missing Fields");
                return View(nameof(Create), createMember);
            }

            bool Result = _memberService.CreateMember(createMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to be Created, check phone, email, or photo validation";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Cannot be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberForUpdate(id);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var result = _memberService.UpdateMemberDetails(id, viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed to be Updated, check phone and mail";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Cannot be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;

            return View();
        }

        public ActionResult DeleteConfirm(int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed to be Deleted";
            }
            return RedirectToAction(nameof(Index));
        }
            #endregion
    }
}
