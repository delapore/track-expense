using System.Text.Json.Serialization;
using ExpenseApi.Repositories;
using ExpenseApi.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using NLog;
using ExpenseApi.Logger;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ExpenseApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddDbContext<ExpenseContext>(c =>
            {
                c.UseSqlServer(Configuration["ConnectionString"]);
            });

            services.AddTransient<IExpenseRepository, ExpenseRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(sg =>
            {
                sg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "trackExpense - Expense HTTP API",
                    Version = "v1",
                    Description = "Expense Data-Driven Microservice HTTP API."
                });
            });

            services.AddCors();

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.PostConfigure((System.Action<ApiBehaviorOptions>)(options =>
            {
                var builtInFactory = options.InvalidModelStateResponseFactory;
                options.InvalidModelStateResponseFactory = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerManager>();
                    logger.LogModelState(context.ModelState);
                    return builtInFactory(context);
                };
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense API V1");
            });
        }
    }
}
