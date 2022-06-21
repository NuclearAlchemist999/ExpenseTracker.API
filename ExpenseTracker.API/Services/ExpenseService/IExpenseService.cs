using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;

namespace ExpenseTracker.API.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, string cookie);
        Task<AllExpensesResponseDto> GetAllExpensesByYearAndMonth(string month, string year, string cookie);
    }
}
