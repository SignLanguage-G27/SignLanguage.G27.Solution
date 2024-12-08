using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using SignLanguage.Application;
using SignLanguage.Core.Entities.Identity;
using SignLanguage.Core.Service.Contract;
using SignLanguage.Infrastracture.Data.Identity;

namespace SignLanguage.APIs.Extenstions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services) 
        {
           services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddIdentity<AppUser, IdentityRole>()
                                          .AddEntityFrameworkStores<AppIdentityDbContext>();
            return services;
        }
    }
}
