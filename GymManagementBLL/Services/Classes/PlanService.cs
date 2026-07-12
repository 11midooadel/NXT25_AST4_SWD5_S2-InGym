using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostitories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWorks.GetRepository<Plan>().GetAll();
            if (Plans == null || !Plans.Any()) return [];

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(Plans);
        }

        public PlanViewModel GetPlanDetails(int PlanId)
        {
            var Plan = _unitOfWorks.GetRepository<Plan>().GetById(PlanId);
            if (Plan == null) return null;

            return _mapper.Map<PlanViewModel>(Plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            Console.WriteLine($"Requested ID = {PlanId}");
            var Plan = _unitOfWorks.GetRepository<Plan>().GetById(PlanId);
            Console.WriteLine($"Found Plan: {Plan?.Name ?? "NULL"}");
            if (Plan is null || Plan.IsActive == false || HasActiveMemberShips(PlanId)) return null;

            return _mapper.Map<UpdatePlanViewModel>(Plan);
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            try
            {
                var PlanRepo = _unitOfWorks.GetRepository<Plan>();
                var Plan = PlanRepo.GetById(PlanId);
                if (Plan is null || HasActiveMemberShips(PlanId)) return false;

                (Plan.Description, Plan.DurationDays, Plan.Price, Plan.UpdatedAt) = (updatedPlan.Description, updatedPlan.DurationDays, updatedPlan.Price, DateTime.Now);

                PlanRepo.Update(Plan);
                return _unitOfWorks.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool ToggleStatus(int PlanId)
        {
            try
            {
                var PlanRepo = _unitOfWorks.GetRepository<Plan>();
                var Plan = PlanRepo.GetById(PlanId);
                if (Plan is null || HasActiveMemberShips(PlanId)) return false;

                Plan.IsActive = Plan.IsActive == true ? false : true;

                PlanRepo.Update(Plan);
                return _unitOfWorks.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool HasActiveMemberShips(int PlanId)
        {
            return _unitOfWorks.GetRepository<MemberShip>().GetAll(X => X.PlanId == PlanId && X.Status == "Active").Any();
        } 
        #endregion
    }
}
