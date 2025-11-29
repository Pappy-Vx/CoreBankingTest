using CoreBankingTest.Application.Accounts.Commands.CreateAccount;
using CoreBankingTest.Application.Common.Behaviors;
using CoreBankingTest.Application.Common.Mappings;
using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Infrastructure.Data;
using CoreBankingTest.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CoreBankingTest.API.Middleware;
using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


namespace CoreBankingTest.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BankingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register dependencies (DI)
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddValidatorsFromAssembly(typeof(CreateAccountCommandValidator).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // Add services to the container.
            builder.Services.AddControllers();

            // Add MediatR with behaviours
            builder.Services.AddMediatR(cfg =>
            {
                // Note: Registering one command is enough per Layer�MediatR scans the entire Application assembly (all Commands & Queries).
                cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommand).Assembly);

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));

                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            // Add AutoMapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(AccountProfile).Assembly);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Enriched swaggerGen with XML comments and authentication
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CoreBanking API",
                    Version = "v1",
                    Description = "A modern banking API built with Clean Architecture, DDD and CQRS",
                    Contact = new OpenApiContact
                    {
                        Name = "CoreBanking Team",
                        Email = "support@corebanking.com"
                    }
                });

                // Include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Add authentication support in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);

                // Enriched Swagger UI
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreBanking API v1");
                    c.RoutePrefix = "swagger"; // Access at /swagger
                    c.DocumentTitle = "CoreBanking API Documentation";
                    c.EnableDeepLinking();
                    c.DisplayOperationId();
                });
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
