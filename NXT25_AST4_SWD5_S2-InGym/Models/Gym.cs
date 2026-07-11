using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class Gym
    {
        [Key]
        public int GymID { get; set; }

        public string GymName { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }

        public ICollection<GymLocation> GymLocations { get; set; } = new List<GymLocation>();

        public ICollection<GymCoach> GymCoaches { get; set; } = new List<GymCoach>();

        public ICollection<GymMember> GymMembers { get; set; } = new List<GymMember>();

        public ICollection<MemberGymSub> MemberGymSubs { get; set; } = new List<MemberGymSub>();
        public ICollection<GymManager> GymManagers { get; set; } = new List<GymManager>();
    }
}