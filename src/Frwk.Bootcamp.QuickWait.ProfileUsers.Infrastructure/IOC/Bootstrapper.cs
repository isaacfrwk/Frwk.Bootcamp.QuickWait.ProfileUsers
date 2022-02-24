using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Consumers;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces.Service;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.UserContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.IOC
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddRepositoriesProfileUsers(this IServiceCollection services)
            => services
                .AddScoped<IUserRepository, UserRepository>();



        public static IServiceCollection AddServicesProfileUsers(this IServiceCollection services)
           => services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IProduceService, ProduceService>();
            

        public static IServiceCollection AddHostedService(this IServiceCollection services)
            => services
                .AddHostedService<UserConsumer>();

        public static void AddHealthCheckConfguration(this IServiceCollection services, IConfiguration configuration)
            => services.AddHealthChecks()
                       .AddSqlServer(connectionString: configuration.GetConnectionString("ConnectionString"), name: "Instancia do sql server");

    }                                                   
}
