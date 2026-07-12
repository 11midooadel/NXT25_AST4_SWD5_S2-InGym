using AutoMapper;
using GymManagementBLL.Services.Attachment;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostitories.Classes;
using GymManagementDAL.Repostitories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachment;

        public MemberService(IUnitOfWorks unitOfWorks, IMapper mapper, IAttachmentService attachment)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _attachment = attachment;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWorks.GetRepository<Member>().GetAll();
            if (Members == null || !Members.Any()) return [];

            var MemberViewModels = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(Members);

            return MemberViewModels;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone)) return false;

                var PhotoName = _attachment.Upload("Members", createMember.PhotoFile);

                if (string.IsNullOrEmpty(PhotoName)) return false;

                var Member = _mapper.Map<CreateMemberViewModel, Member>(createMember);
                Member.Photo = PhotoName;


                _unitOfWorks.GetRepository<Member>().Add(Member);
                var IsCreated = _unitOfWorks.SaveChanges() > 0;
                if (!IsCreated)
                {
                    _attachment.Delete(PhotoName, "Member");
                }
                return IsCreated;
            }
            catch
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _unitOfWorks.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            var  memberViewModel = _mapper.Map<MemberViewModel>(member);

            var ActiveMemberShip = _unitOfWorks.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();

            if (ActiveMemberShip is not null)
            {
                memberViewModel.MemberShipStartDate = ActiveMemberShip.CreateAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                var plan = _unitOfWorks.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                memberViewModel.PlanName = plan?.Name;
            }
            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int MemberId)
        {
            var memberHealthRecord = _unitOfWorks.GetRepository<HealthRecord>().GetById(MemberId);

            if (memberHealthRecord == null) return null;

            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberForUpdate(int MemberId)
        {
            var member = _unitOfWorks.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                var emailExist = _unitOfWorks.GetRepository<Member>().GetAll(X => X.Email == memberToUpdate.Email && X.Id != Id).Any();

                var phoneExist = _unitOfWorks.GetRepository<Member>().GetAll(X => X.Phone == memberToUpdate.Phone && X.Id != Id).Any();

                if (emailExist || phoneExist) return false;

                var MemberRepo = _unitOfWorks.GetRepository<Member>();
                var member = MemberRepo.GetById(Id);
                if (member == null) return false;
                 

                _mapper.Map(memberToUpdate, member);

                MemberRepo.Update(member);
                return _unitOfWorks.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }

        }

        public bool RemoveMember(int MemberId)
        {
            try
            {
                var member = _unitOfWorks.GetRepository<Member>().GetById(MemberId);
                if (member == null) return false;

                var SessionsIds = _unitOfWorks.GetRepository<MemberSession>().GetAll(X => X.MemberId == MemberId).Select(X => X.SessionId);

                var ActiveMemberSession = _unitOfWorks.GetRepository<Session>().GetAll(X => SessionsIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();



                if (ActiveMemberSession) return false;



                var memberShipRepo = _unitOfWorks.GetRepository<MemberShip>();
                var memberShips = memberShipRepo.GetAll(X => X.MemberId == MemberId);

                if (memberShips.Any())
                {
                    foreach (var memberShip in memberShips)
                    {
                        memberShipRepo.Delete(memberShip);
                    }
                }

                _unitOfWorks.GetRepository<Member>().Delete(member);
                var IsDeleted = _unitOfWorks.SaveChanges() > 0;

                if (IsDeleted)
                {
                    _attachment.Delete(member.Photo, "Members");
                }
                return IsDeleted;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            return _unitOfWorks.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        }

        private bool IsPhoneExist(string phone)
        {
            return _unitOfWorks.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        }
        #endregion
    }
}
