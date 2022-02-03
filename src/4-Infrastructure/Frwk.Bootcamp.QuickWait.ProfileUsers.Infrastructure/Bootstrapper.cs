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
        {
            services
                .AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddServicesProfileUsers(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>();

            return services;
        }
    }                                                   
}
