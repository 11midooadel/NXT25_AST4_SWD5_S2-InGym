using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class Coach
    {
        [Key]
        public int CoachID { get; set; }

        public decimal Salary { get; set; }

        public string Speciality { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public double Rating { get; set; }

        public int UserID { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Member> Members { get; set; } = new List<Member>();

        public ICollection<GymCoach> GymCoaches { get; set; } = new List<GymCoach>();

        public ICollection<MemberPrivateSub> MemberPrivateSubs { get; set; } = new List<MemberPrivateSub>();
    }
}