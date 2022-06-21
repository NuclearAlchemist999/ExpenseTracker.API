using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Services.AccountService
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccount(CreateAccountRequestDto request);
        Task<Account> GetAccount(string username);
        Task<AccountDto> GetAccountById(string cookie);
    }
}
