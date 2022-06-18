namespace ExpenseTracker.API.Services.JwtService
{
    public interface IJwtService
    {
        string CreateToken(Guid accountId);
    }
}
