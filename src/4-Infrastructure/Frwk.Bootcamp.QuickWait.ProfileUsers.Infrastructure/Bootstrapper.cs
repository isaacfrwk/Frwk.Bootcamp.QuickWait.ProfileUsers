using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Consumers;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.UserContext;
using Microsoft.Extensions.DependencyInjection;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddRepositoriesProfileUsers(this IServiceCollection services)
            => services
                .AddScoped<IUserRepository, UserRepository>();



        public static IServiceCollection AddServicesProfileUsers(this IServiceCollection services)
           => services
                .AddScoped<IUserService, UserService>();


        public static IServiceCollection AddHostedService(this IServiceCollection services)
            => services
                .AddHostedService<UserConsumer>();

    }                                                   
}
