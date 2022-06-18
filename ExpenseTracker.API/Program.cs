using ExpenseTracker.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureCors();

builder.Services.ConfigureDatabase(builder.Configuration);

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
