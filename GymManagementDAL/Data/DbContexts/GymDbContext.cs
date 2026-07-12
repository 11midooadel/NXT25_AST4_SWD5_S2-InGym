using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options): base(options)
        {

        }
        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = .;Database = GymManagementDB; Trusted_Connection = True; TrustServerCertificate = True;");
        //}

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(Eb =>
            {
                Eb.Property(X => X.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);
                Eb.Property(X => X.LastName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);
            });
        }

        #region Db Sets
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Trainer> Trainers { get; set; } = null!;
        public DbSet<Plan> Plans { get; set; } = null!;
        public DbSet<MemberShip> MemberShips { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<MemberSession> MemberSessions { get; set; } = null!;
        public DbSet<HealthRecord> HealthRecords { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        #endregion
    }
}
