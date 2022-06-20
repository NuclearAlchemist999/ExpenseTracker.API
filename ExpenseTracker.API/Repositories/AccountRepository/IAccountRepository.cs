using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Repositories.AccountRepository 
{
    public interface IAccountRepository 
    {
        Task<Account> CreateAccount(Account account);
        Task<Account> GetAccountByUsername(string username);
        Task<Account> GetAccountById(Guid accountId);
    }
}
