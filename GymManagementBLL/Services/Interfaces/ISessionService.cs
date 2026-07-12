using GymManagementBLL.Services.Classes;
using GymManagementBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel GetSessionById(int id);
        bool CreateSession(CreateSessionViewModel createSession);
        UpdateSessionViewModel GetSessionToUpdate(int id);
        bool UpdateSession(int id, UpdateSessionViewModel updateSession);
        bool DeleteSession(int id);

        IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetAllCategoryForDropDown();
    }
}
