using ExpenseTracker.API;
using ExpenseTracker.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureCors();

builder.Services.ConfigureDatabase(builder.Configuration);

builder.Services.ConfigureServices();

builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

app.ConfigureExceptionHandler();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
