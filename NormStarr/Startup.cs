
using System.Linq;
using System.Threading.Tasks;
using Data.ClassesForInterfaces;
using Data.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NormStarr.ErrorHandling;
using NormStarr.Extensions;
using NormStarr.MiddleWare;
using StackExchange.Redis;

namespace NormStarr
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataInitialize,DataInitialize>();
            services.AppService(Configuration);
            services.AddControllers();
  
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "NormStarr", Version = "v1" });
            // });
            services.AddCors(options =>
            {
                options.AddPolicy("CosmicDesigns", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataInitialize data)
        {
            
            app.UseMiddleware<ExceptionMiddleWare>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            if (env.IsDevelopment())
            {
               
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NormStarr v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting(); 
            app.UseCors("CosmicDesigns");
            data.Initialize();
            // app.UseAuthentication();
            // app.UseAuthorization();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("index","FallBack");
            });
        }
    }
}
