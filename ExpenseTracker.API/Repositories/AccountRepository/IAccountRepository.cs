using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Repositories.AccountRepository 
{
    public interface IAccountRepository 
    {
        Task<Account> CreateAccount(Account account);
    }
}
