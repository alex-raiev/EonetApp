using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using EonetApp.Clients;
using EonetApp.Configuration;
using EonetApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace EonetApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<UrlsConfiguration>(Configuration.GetSection("Urls"));
            services.Configure<UrlsConfiguration>(Configuration.GetSection("Redis"));
            
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EONET Test App", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var commentsXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentsXmlPath = Path.Combine(AppContext.BaseDirectory, commentsXmlFile);

                c.IncludeXmlComments(commentsXmlPath);
            });

            services.AddMemoryCache(options =>
            {
                
            });

            services.AddScoped<IEonetService, EonetService>()
                .AddScoped<IEonetTrackerClient, EonetTrackerClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eonet Test App v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
