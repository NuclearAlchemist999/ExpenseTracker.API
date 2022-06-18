using ExpenseTracker.API.DTO.Request;

namespace ExpenseTracker.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> ValidateLogin(LoginRequestDto request);
    }
}
