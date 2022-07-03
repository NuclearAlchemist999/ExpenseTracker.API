using ExpenseTracker.API.Data;
using ExpenseTracker.API.Repositories.AccountRepository;
using ExpenseTracker.API.Services.AccountService;
using ExpenseTracker.API.Services.AuthService;
using ExpenseTracker.API.Services.JwtService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ExpenseTracker.API.Repositories.ExpenseRepository;
using ExpenseTracker.API.Services.ExpenseService;
using ExpenseTracker.API.Repositories.CategoryRepository;
using ExpenseTracker.API.Services.CategoryService;

namespace ExpenseTracker.API
{
    public static class RegisterDependencies
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG
            var connectionString = configuration["ConnectionStrings:Elephant"];
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            var cookies = context.Request.Cookies;  

                            if (cookies.TryGetValue("authToken", out var authToken))
                            {
                                context.Token = authToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
#if DEBUG
                    var secretKey = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]);
#else
                    var secretKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY"));
#endif
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
