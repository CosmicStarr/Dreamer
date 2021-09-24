using Data;
using Data.ClassesForInterfaces;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using NormStarr.AutoMapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NormStarr.EmailSenderServices;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace NormStarr.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AppService(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMailJetEmailSender,MailJetSender>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IApplicationUserRepo,ApplicationUserRepo>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapProfiles));
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<AppUser,IdentityRole>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = config["JWT:VaildAudience"],
                    ValidIssuer = config["JWT:VaildIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]))

                };
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("ManagerDevelopers", o =>
                {
                    o.RequireClaim("JobDepartment","Developer");
                    o.RequireRole("Manager");
                });
                opt.AddPolicy("AdminDevelopers", o =>
                {
                    o.RequireClaim("JobDepartment","Developer");
                    o.RequireRole("Admin");
                });
            });
     
            return services;   
        }
    }
}