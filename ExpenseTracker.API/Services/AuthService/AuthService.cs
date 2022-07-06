using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Exceptions;
using ExpenseTracker.API.Repositories.AccountRepository;

namespace ExpenseTracker.API.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepo;

        public AuthService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<bool> ValidateLogin(LoginRequestDto request)
        {
            var account = await _accountRepo.GetAccountByUsername(request.Username);

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(request.Password, account.Password);

            if (!isCorrectPassword)
            {
                throw new InvalidCredentialsException();
            }

            return isCorrectPassword;
        }
    }
}
