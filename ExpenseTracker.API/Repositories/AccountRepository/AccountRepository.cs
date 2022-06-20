using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Account> GetAccountByUsername(string username)
        {
            return await _exTrackContext.Accounts.FirstOrDefaultAsync(account => account.Username == username);
        }

        public async Task<Account> GetAccountById(Guid accountId)
        {
            return await _exTrackContext.Accounts.FindAsync(accountId);
        }
    }
}
