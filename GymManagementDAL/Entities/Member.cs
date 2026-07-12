using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // JoinDate == CreateAt (By Fluent API)
        public string Photo { get; set; } = null!;

        #region RelationShip
        #region Member - HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShip
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion

        #region Member - Session
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion
        #endregion
    }
}
