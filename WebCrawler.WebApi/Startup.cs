using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using WebCrawler.EntityFramework;
using WebCrawler.Services.Extensions;
using WebCrawler.WebApi.Middlewares;
using WebCrawler.WebApi.Services;

namespace WebCrawler.WebApi
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
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebCrawler.WebApi", Version = "v1" });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services.AddWebCrawler();
            services.AddEfRepository<ApplicationDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("connectionString")));
            services.AddWebCrawlerServices();
            services.AddScoped<MapperService>();
            services.AddScoped<TestsService>();
            services.AddScoped<TestDetailsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebCrawler.WebApi v1"));
            }

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin()
                                          .AllowAnyHeader()
                                          .AllowAnyMethod()
                                          );

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
