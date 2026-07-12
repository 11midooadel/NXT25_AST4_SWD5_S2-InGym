using GymManagementBLL;
using GymManagementBLL.Services.Attachment;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostitories.Classes;
using GymManagementDAL.Repostitories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region ConfigurationService
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                Config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();
            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Account/Login";
                opt.AccessDeniedPath = "/Account/AccessDenied";
            });
            builder.Services.AddIdentityCore<ApplicationUser>().AddEntityFrameworkStores<GymDbContext>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            #endregion
            var app = builder.Build();
            #region Seed Data - Migrate Database
            var scoped = app.Services.CreateScope();
            var dbContext = scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();
            GymDbContextDataSeeding.SeedData(dbContext);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);
            #endregion

            #region Configure Kestral Middlewares
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id:int?}")
                .WithStaticAssets();
            #endregion

            app.Run();
        }
    }
}
