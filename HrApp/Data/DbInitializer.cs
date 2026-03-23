using HrApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Data
{
    public static class DbInitializer
    {
        public static void EnsureDbMigrated(this IApplicationBuilder applicationBuilder)
        {
            //Create a temporary scope
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Get instance from DI container for AppDbContext:
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.Migrate();
                if (!context.Employees.Any())
                {
                    var employee = new Employee { FirstName = "Wim", LastName = "Bervoets" };
                    context.Employees.Add(employee);
                    employee = new Employee { FirstName = "Kristof", LastName = "Palmaers" };
                    context.Employees.Add(employee);
                    context.SaveChanges();
                }
            }
        }
    }
}
