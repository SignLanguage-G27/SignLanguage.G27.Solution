﻿using Microsoft.AspNetCore.Builder;
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
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https//localhost:4200");
                });
            });

           services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;          // لا تطلب أرقامًا
                options.Password.RequiredLength = 0;           // الحد الأدنى للطول
                options.Password.RequireLowercase = false;     // لا تطلب حروف صغيرة
                options.Password.RequireUppercase = false;     // لا تطلب حروف كبيرة
                options.Password.RequireNonAlphanumeric = false; // لا تطلب رموزًا خاصة
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            return services;
        }
    }
}
