using GymManagementDAL.Data.DbContexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();

                if (HasPlans == true && HasCategories == true) return false;
                if (!HasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        dbContext.Plans.AddRange(plans);
                }
                if (!HasCategories)
                {
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any())
                        dbContext.Categories.AddRange(Categories);
                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }
        }

        private static List<T> LoadDataFromJsonFile<T> (string fileName)
        {
            // D:\Coding\Route\02 - Back-End\07 ASP .Net MVC\GymManagementSystemSolution\GymManagementPL\wwwroot\Files\plans.json
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            string Date = File.ReadAllText(FilePath);

            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(Date, Options) ?? new List<T>();
        }
    }
}
