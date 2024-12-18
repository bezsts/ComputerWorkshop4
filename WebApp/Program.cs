using ApiDomain;
using ApiDomain.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using WebApp;
using WebApp.Options;
using WebApp.Options.Validators;

namespace ComputerWorkshop2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddExporters();

            builder.Services.AddOptionsValidators();
            builder.Services.AddOptionsWithValidateOnStart<ExportMoviesOptions>().BindConfiguration("ExportMoviesOptions");
            builder.Services.AddOptionsWithValidateOnStart<CsvOptions>().BindConfiguration("ExportMoviesOptions:CsvOptions");

            builder.Services.AddDbContext<DataModelContext>(contextOptions =>
                contextOptions.UseSqlite("Data Source=Database/sample.db"));

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddFluentValidationAutoValidation();

            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Version = "v1",
            //        Title = "Catalog of watched films API",
            //        Description = "An ASP.NET Core Web API for managing a catalog of watched films"
            //    });

            //    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
            //        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            //});

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSerilogRequestLogging();
            app.MapControllers();
            app.UseCors();

            app.Run();
        }
    }
}
