using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MagicVilla_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddHttpClient<IVillaService, VillaService>();
            builder.Services.AddScoped<IVillaService, VillaService>();
            builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
            builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.Cookie.HttpOnly = true;
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    option.LoginPath = "/Auth/Login";
                    option.AccessDeniedPath = "/Auth/AccessDenied";
                    option.SlidingExpiration = true;
                });
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(100);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("mypolicy", options =>
                {
                    options.AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("mypolicy");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
