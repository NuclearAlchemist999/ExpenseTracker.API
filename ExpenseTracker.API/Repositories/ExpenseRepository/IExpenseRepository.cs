using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<Expense> GetExpense(Guid expenseId);
        Task<Expense> AddExpense(Expense expense);
        Task<List<Expense>> GetExpensesByYearAndMonth(Guid accountId, ExpenseParams _params, string shortMonth,
            bool withPages);      
        Task<bool> DeleteExpense(Guid expenseId);
        Task<Expense> UpdateExpense(Guid id, UpdateExpenseRequestDto request);
        Task<List<Expense>> GetExpenses(Guid accountId, ExpenseParams _params);
        Task<List<Expense>> GetExpensesByCategories(Guid accountId, ExpenseParams _params, bool withPages);
        Task<List<Expense>> GetExpensesByTimeInterval(Guid accountId, ExpenseParams _params, bool withPages);
        Task<List<Expense>> GetExpensesByTimeIntervalAndCategories(Guid accountId, ExpenseParams _params,
            bool withPages);
        Task<List<Expense>> GetExpensesByYear(Guid accountId, ExpenseParams _params, bool withPages);
    }
}
