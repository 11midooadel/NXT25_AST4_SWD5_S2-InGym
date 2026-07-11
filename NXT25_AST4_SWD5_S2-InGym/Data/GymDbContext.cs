using NXT25_AST4_SWD5_S2_InGym.Models;
using Microsoft.EntityFrameworkCore;

namespace NXT25_AST4_SWD5_S2_InGym.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<GymLocation> GymLocations { get; set; }
        public DbSet<GymManager> GymManagers { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<GymSub> GymSubs { get; set; }
        public DbSet<PrivateSub> PrivateSubs { get; set; }
        public DbSet<HealthMetricLog> HealthMetricLogs { get; set; }
        public DbSet<DietaryPlan> DietaryPlans { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<GymCoach> GymCoaches { get; set; }
        public DbSet<GymMember> GymMembers { get; set; }
        public DbSet<MemberHealthMetric> MemberHealthMetrics { get; set; }
        public DbSet<MemberDietaryPlan> MemberDietaryPlans { get; set; }
        public DbSet<MemberWorkoutPlan> MemberWorkoutPlans { get; set; }
        public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
        public DbSet<MemberGymSub> MemberGymSubs { get; set; }
        public DbSet<MemberPrivateSub> MemberPrivateSubs { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Keys
            modelBuilder.Entity<GymCoach>()
                .HasKey(x => new { x.GymID, x.CoachID });

            modelBuilder.Entity<GymMember>()
                .HasKey(x => new { x.GymID, x.MemberID });

            modelBuilder.Entity<MemberDietaryPlan>()
                .HasKey(x => new { x.MemberID, x.DietID });

            modelBuilder.Entity<MemberWorkoutPlan>()
                .HasKey(x => new { x.MemberID, x.PlanID });
            modelBuilder.Entity<MemberWorkoutPlan>()
    .HasOne(x => x.Member)
    .WithMany(x => x.MemberWorkoutPlans)
    .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<MemberWorkoutPlan>()
                .HasOne(x => x.WorkoutPlan)
                .WithMany(x => x.MemberWorkoutPlans)
                .HasForeignKey(x => x.PlanID);

            modelBuilder.Entity<MemberHealthMetric>()
                .HasKey(x => new { x.MemberID, x.MetricID });

            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasKey(x => new { x.PlanID, x.ExerciseID });
            modelBuilder.Entity<WorkoutPlanExercise>()
    .HasOne(x => x.WorkoutPlan)
    .WithMany(x => x.WorkoutPlanExercises)
    .HasForeignKey(x => x.PlanID);

            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(x => x.Exercise)
                .WithMany(x => x.WorkoutPlanExercises)
                .HasForeignKey(x => x.ExerciseID);


            modelBuilder.Entity<MemberGymSub>()
                .HasKey(x => new { x.MemberID, x.GymSubID });

            modelBuilder.Entity<MemberPrivateSub>()
                .HasKey(x => new { x.MemberID, x.PrivateSubID });

            modelBuilder.Entity<UserPhone>()
                .HasKey(x => new { x.UserID, x.Phone });

            // One To One
            modelBuilder.Entity<Member>()
                .HasOne(x => x.User)
                .WithOne(x => x.Member)
                .HasForeignKey<Member>(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Coach>()
                .HasOne(x => x.User)
                .WithOne(x => x.Coach)
                .HasForeignKey<Coach>(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GymManager>()
                .HasOne(x => x.User)
                .WithOne(x => x.GymManager)
                .HasForeignKey<GymManager>(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // One To Many
            modelBuilder.Entity<Member>()
                .HasOne(x => x.Coach)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.CoachID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(x => x.Member)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal Precision
            modelBuilder.Entity<Coach>()
                .Property(x => x.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GymManager>()
                .Property(x => x.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GymSub>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrivateSub>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);
            modelBuilder.Entity<GymManager>()
            .HasOne(gm => gm.Gym)
        .WithMany(g => g.GymManagers)
        .HasForeignKey(gm => gm.GymID)
        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Admin>()
    .HasOne(a => a.User)
    .WithOne(u => u.Admin)
    .HasForeignKey<Admin>(a => a.UserID)
    .OnDelete(DeleteBehavior.Restrict);
        }
           
    }






}


