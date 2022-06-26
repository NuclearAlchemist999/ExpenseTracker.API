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
        Task<AllExpensesResponseDto> GetAllExpensesByYearAndMonth(ExpenseParams param, string cookie);
        Task<Expense> GetExpense(Guid expenseId);
        Task<bool> DeleteExpense(Guid id);
        Task<Expense> UpdateExpense(Guid id, UpdateExpenseRequestDto request);
    }
}
