using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddServicesProfileUsers(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
