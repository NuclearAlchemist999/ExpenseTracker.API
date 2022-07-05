using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;

namespace ExpenseTracker.API.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, string cookie);
        Task<ExpenseDto> GetExpense(Guid expenseId);
        Task<bool> DeleteExpense(Guid id);
        Task<ExpenseDto> UpdateExpense(Guid id, UpdateExpenseRequestDto request);
        Task<AllExpensesResponseDto> FilterExpenses(ExpenseParams _params, string cookie);
        List<string> ValidateFilterParams(ExpenseParams _params);
    }
}
