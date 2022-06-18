using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ExTrackerDbContext _exTrackContext;

        public AccountRepository(ExTrackerDbContext exTrackContext)
        {
            _exTrackContext = exTrackContext;
        }

        public async Task<Account> CreateAccount(Account account)
        {
            _exTrackContext.Accounts.Add(account);

            await _exTrackContext.SaveChangesAsync();

            return account;
        }
    }
}
