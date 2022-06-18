using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.DTO.Converters
{
    public static class AccountConverter
    {
        public static AccountDto ToAccountDto(this Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                Username = account.Username,
                Theme = account.Theme
            };
        }
    }
}
