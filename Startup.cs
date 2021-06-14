using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

namespace EonetApp
{
    using Clients;
    using Configuration;
    using Services;
    using Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IEdmModel GetModel()
            {
                var odataBuilder = new ODataConventionModelBuilder();
                odataBuilder.EntitySet<Event>("Event");

                return odataBuilder.GetEdmModel();
            }

            services.Configure<UrlsConfiguration>(Configuration.GetSection("Urls"));
            services.Configure<UrlsConfiguration>(Configuration.GetSection("Redis"));
            
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddOData(options => options.AddModel("odata", GetModel()).Filter().Select().Expand());
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EONET Test App", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var commentsXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentsXmlPath = Path.Combine(AppContext.BaseDirectory, commentsXmlFile);

                c.IncludeXmlComments(commentsXmlPath);
            });
            
            services.AddMemoryCache();

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
