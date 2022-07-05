using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetExpenses(Guid accountId, string orderBy);
        Task<Expense> GetExpense(Guid expenseId);
        Task<Expense> AddExpense(Expense expense);    
        Task<bool> DeleteExpense(Guid expenseId);
        Task<Expense> UpdateExpense(Expense expense);
    }
}
