using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.Repositories.AccountRepository;

namespace ExpenseTracker.API.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<AccountDto> CreateAccount(CreateAccountRequest request)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = hashedPassword
            };

            var createdAccount = await _accountRepo.CreateAccount(newAccount);

            return createdAccount.ToAccountDto();
        }

        public async Task<Account> GetAccount(string username)
        {
            return await _accountRepo.GetAccountByUsername(username);
        }
    }
}
