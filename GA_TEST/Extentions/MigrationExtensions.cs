using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using GA_TEST.DbContexts;

namespace GA_TEST.Extentions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using CityInfoContext dbContext =
                scope.ServiceProvider.GetRequiredService<CityInfoContext>();

            dbContext.Database.Migrate();
        }
    }
}

