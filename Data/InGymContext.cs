using Microsoft.EntityFrameworkCore;
using InGym.Models;

namespace InGym.Data
{
    public class InGymContext : DbContext
    {
        public InGymContext(DbContextOptions<InGymContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ProgressLog> ProgressLogs { get; set; }
        public DbSet<Equipment> Equipment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Equipment: Shelf_Code as primary key (already configured via [Key] annotation)
            modelBuilder.Entity<Equipment>()
                .HasKey(e => e.Shelf_Code);

            // Subscription relationships
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProgressLog relationships
            modelBuilder.Entity<ProgressLog>()
                .HasOne(p => p.Trainee)
                .WithMany()
                .HasForeignKey(p => p.TraineeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProgressLog>()
                .HasOne(p => p.Trainer)
                .WithMany()
                .HasForeignKey(p => p.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
