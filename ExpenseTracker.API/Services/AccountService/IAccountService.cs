using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;

namespace ExpenseTracker.API.Services.AccountService
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccount(CreateAccountRequest request);
    }
}
