using BL.IRepositories;
using BL.Models;
using BL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer("name=ConnectionStrings:DbConnStr");
            });

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication()
                .AddCookie(options => {
                    options.LoginPath = "/User/Login";
                    options.LogoutPath = "/User/Logout";
                    options.AccessDeniedPath = "/User/Forbidden";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BL.Mapping.MappingProfile>();
                cfg.AddProfile<WebApp.Mapping.MappingProfile>();
            });

            var secureKey = builder.Configuration["JWT:SecureKey"];

            builder.Services.AddScoped<ITravelRepository, TravelRepository>();
            builder.Services.AddScoped<ILogsRepository, LogsRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
            builder.Services.AddScoped<IGuideRepository, GuideRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
