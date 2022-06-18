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

        public async Task<AccountDto>
    }
}
