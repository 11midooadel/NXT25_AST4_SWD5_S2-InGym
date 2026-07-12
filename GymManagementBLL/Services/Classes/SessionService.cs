using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostitories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public ISessionRepository SessionRepository { get; }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWorks.SessionRepository.GetAllSessionsWithTrainerAndCategory();

            if (Sessions == null || !Sessions.Any()) return [];

            var mappedSession = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            return mappedSession;
        }

        public SessionViewModel GetSessionById(int id)
        {
            var Sessions = _unitOfWorks.SessionRepository.GetSessionByIdWithTrainerAndCategory(id);
            if (Sessions == null) return null;

            #region Manual Mapping
            // Manual Mapping
            //return new SessionViewModel
            //{
            //    Capacity = Session.Capacity,
            //    Description = Session.Description,
            //    EndDate = Session.EndDate,
            //    StartDate = Session.StartDate,
            //    TrainerName = Session.SessionTrainer.Name,
            //    CategoryName = Session.SessionCategory.CategoryName,
            //    AvailableSlots = Session.Capacity - _unitOfWorks.SessionRepository.GetCountOfBookedSlots(Session.Id)
            //};
            #endregion

            #region Automatic Mapping
            var mappedSession = _mapper.Map<Session, SessionViewModel>(Sessions);
            return mappedSession; 
            #endregion
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;

                var mappedSessions = _mapper.Map<CreateSessionViewModel, Session>(createSession);

                _unitOfWorks.SessionRepository.Add(mappedSessions);

                return _unitOfWorks.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public UpdateSessionViewModel GetSessionToUpdate(int id)
        {
            var Sessions = _unitOfWorks.SessionRepository.GetById(id);
            if (!IsSessionAvailableForUpdate(Sessions!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Sessions!);
        }

        public bool UpdateSession(int id, UpdateSessionViewModel updateSession)
        {
            try
            {
                var Sessions = _unitOfWorks.SessionRepository.GetById(id);
                if (IsSessionAvailableForUpdate(Sessions!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;

                _mapper.Map(updateSession, Sessions);
                Sessions!.UpdatedAt = DateTime.Now;
                return _unitOfWorks.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSession(int id)
        {
            try
            {
                var Sessions = _unitOfWorks.SessionRepository.GetById(id);
                if (!IsSessionAvailableForDeleting(Sessions!)) return false;

                _unitOfWorks.SessionRepository.Delete(Sessions!);
                return _unitOfWorks.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown()
        {
            var Trainers = _unitOfWorks.GetRepository<Trainer>().GetAll();

            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetAllCategoryForDropDown()
        {
            var Categories = _unitOfWorks.GetRepository<Category>().GetAll();

            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        #region Helpers
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWorks.GetRepository<Trainer>().GetById(TrainerId) is not null;
            
        }
        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWorks.GetRepository<Category>().GetById(CategoryId) is not null;

        }
        private bool IsValidDateRange(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate && StartDate > DateTime.Now;
        }
        private bool IsSessionAvailableForUpdate(Session session)
        {
            if (session == null) return false;
            if (session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now) return false;
            var HasActiveBookings = _unitOfWorks.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBookings) return false;
            return true;
        }
        private bool IsSessionAvailableForDeleting(Session session)
        {
            if (session == null) return false;
            if (session.StartDate > DateTime.Now && session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            var HasActiveBookings = _unitOfWorks.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBookings) return false;
            return true;
        }
        #endregion
    }
}
