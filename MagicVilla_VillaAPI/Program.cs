
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MagicVilla_VillaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region LoggerInFile
            /*Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();
            builder.Host.UseSerilog();*/
            #endregion
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores <ApplicationDbContext>() ;
            builder.Services.AddResponseCaching();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; 
                options.SubstituteApiVersionInUrl = true;
            });
            var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Default30", new CacheProfile { Duration = 30 });
                //options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Authorization header using Bearer scheme.\r\n\r\n" +
                    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                    "Example: \"Bearer 1234asddf\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {  new OpenApiSecurityScheme
                       {
                          Reference =new OpenApiReference
                          {
                              Type=ReferenceType.SecurityScheme,
                              Id="Bearer"
                          },
                          Scheme="oauth2",
                          Name="Bearer",
                          In=ParameterLocation.Header
                       },
                       new List<string>()
                    }


                });
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Magic Villa V1",
                    Description = "API to manage Villa",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Dotnetmastery",
                        Url = new Uri("https://dotnetmastery.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
                option.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "Magic Villa V2",
                    Description = "API to manage Villa",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Dotnetmastery",
                        Url = new Uri("https://dotnetmastery.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
                    option.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
