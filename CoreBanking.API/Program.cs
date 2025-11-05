using CoreBanking.API.Mapping;
using CoreBanking.API.Middleware;
using CoreBanking.Application.Accounts.Commands.CreateAccount;
using CoreBanking.Application.Common.Behaviors;
using CoreBanking.Application.Common.Mappings;
using CoreBanking.Application.Customers.Commands.CreateCustomer;
using CoreBanking.Core.Interfaces;
using CoreBanking.Infrastructure.Data;
using CoreBanking.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CoreBanking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------------------------------------------------
            // 1️⃣ Add Controllers and Swagger
            // ------------------------------------------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CoreBanking API",
                    Version = "v1",
                    Description = "A modern banking API built with Clean Architecture and CQRS",
                    Contact = new OpenApiContact
                    {
                        Name = "CoreBanking Team",
                        Email = "support@corebanking.com"
                    }
                });

                // Include XML comments for better documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);

                // JWT Bearer Authentication in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer eyJhbGci...')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });

            // ------------------------------------------------------------
            // 2️⃣ Configure Database Context
            // ------------------------------------------------------------
            builder.Services.AddDbContext<BankingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ------------------------------------------------------------
            // 3️⃣ Register Repositories and Unit of Work
            // ------------------------------------------------------------
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ------------------------------------------------------------
            // 4️⃣ Configure AutoMapper (Profiles from multiple assemblies)
            // ------------------------------------------------------------
            builder.Services.AddAutoMapper(cfg => { },
            typeof(AccountProfile).Assembly,
            typeof(CustomerProfile).Assembly,
            typeof(RequestToCommandProfile).Assembly);


            // ------------------------------------------------------------
            // 5️⃣ Register FluentValidation
            // ------------------------------------------------------------
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();

            // ------------------------------------------------------------
            // 6️⃣ Register MediatR + Pipeline Behaviors
            // ------------------------------------------------------------
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateCustomerCommand>();
                cfg.RegisterServicesFromAssemblyContaining<CreateAccountCommand>();

                // Add custom pipeline behaviors (executed in order)
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            // ------------------------------------------------------------
            // 7️⃣ Build the application
            // ------------------------------------------------------------
            var app = builder.Build();

            // ------------------------------------------------------------
            // 8️⃣ Use global exception middleware
            // ------------------------------------------------------------
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // ------------------------------------------------------------
            // 9️⃣ Configure Swagger
            // ------------------------------------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreBanking API v1");
                    c.RoutePrefix = "swagger";
                    c.DocumentTitle = "CoreBanking API Documentation";
                    c.EnableDeepLinking();
                    c.DisplayOperationId();
                });
            }

            // ------------------------------------------------------------
            // 🔟 HTTP Request Pipeline
            // ------------------------------------------------------------
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
