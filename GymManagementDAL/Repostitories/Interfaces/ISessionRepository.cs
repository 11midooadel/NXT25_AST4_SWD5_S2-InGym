using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostitories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        Session? GetSessionByIdWithTrainerAndCategory(int id);
        int GetCountOfBookedSlots(int sessionId);
    }
}
