using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SignLanguage.APIs.Errors;
using SignLanguage.APIs.Extenstions;
using SignLanguage.APIs.Middleware;
using SignLanguage.Application;
using SignLanguage.Core.Entities.Identity;
using SignLanguage.Core.Repository.Contract;
using SignLanguage.Core.Service.Contract;
using SignLanguage.Infrastracture;
using SignLanguage.Infrastracture.Data;
using SignLanguage.Infrastracture.Data.Identity;

namespace SignLanguage.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services
            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(option =>
            {
                option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnectoin"));
            });

            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(option =>
            {
                option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            webApplicationBuilder.Services.AddTransient<IAttachmentService,AttachmentService>();

            webApplicationBuilder.Services.AddIdentityServices();


            webApplicationBuilder.Services.AddHttpClient();
            webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(Options=>
{
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                    .SelectMany(P => P.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToList();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                    
                };
            });

            #endregion

            var app = webApplicationBuilder.Build();

            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var _dbContext=services.GetRequiredService<StoreContext>();
            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var _userManger = services.GetRequiredService<UserManager<AppUser>>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();
                await _identityDbContext.Database.MigrateAsync();
              //  await AppIdentityDbContextSeed.SeedUserAsync(_userManger);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred During Apply Migration");
            }

            #region Configure Kestrel MiddleWeares
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // Configure the HTTP request pipeline.

            app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseCors("CORSPolicy");


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
