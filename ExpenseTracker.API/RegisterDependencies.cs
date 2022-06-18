using ExpenseTracker.API.Data;
using ExpenseTracker.API.Repositories.AccountRepository;
using ExpenseTracker.API.Services.AccountService;
using ExpenseTracker.API.Services.JwtService;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API
{
    public static class RegisterDependencies
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG
            var connectionString = configuration["ConnectionStrings:Heroku"];
#else
            var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
            var connectionString = $"Server={dbHost};Port={dbPort};User Id={dbUser};Password={dbPassword};Database={database}";
#endif
            services.AddDbContext<ExTrackerDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .WithOrigins("http://localhost:3000");
                });
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
