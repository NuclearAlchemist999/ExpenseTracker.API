using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Services.AccountService
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccount(CreateAccountRequest request);
        Task<Account> GetAccount(string username);
        Task<AccountDto> GetAccountById(Guid accountId);
    }
}
