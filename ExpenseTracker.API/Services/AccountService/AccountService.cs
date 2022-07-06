using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Exceptions;
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

        public async Task<AccountDto> CreateAccount(CreateAccountRequestDto request)
        {
            var account = await _accountRepo.GetAccountByUsername(request.Username);

            if (account is not null)
            {
                throw new UsernameTakenException();
            }

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
            var account =  await _accountRepo.GetAccountByUsername(username);

            if (account is null)
            {
                throw new InvalidCredentialsException();
            }

            return account;
        }

        public async Task<AccountDto> GetAccountById(string cookie)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                throw new AccountNotFoundException();
            }
       
            var account = await _accountRepo.GetAccountById(Guid.Parse(cookie));
            
            return account.ToAccountDto();
        }
    }
}
