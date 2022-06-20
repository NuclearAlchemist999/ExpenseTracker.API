using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;

namespace ExpenseTracker.API.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, Guid accountId);
    }
}
