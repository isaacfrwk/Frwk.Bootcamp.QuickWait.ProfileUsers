using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.IOC
{
    public static class DataBaseConfigurations
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection service, IConfiguration configuration)
             => service.AddDbContext<DBContext>(options =>
                      options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        public static async Task UseDatabaseConfiguration(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DBContext>();
            await context.Database.MigrateAsync();
            await context.Database.EnsureCreatedAsync();
        }
        
    }
}
