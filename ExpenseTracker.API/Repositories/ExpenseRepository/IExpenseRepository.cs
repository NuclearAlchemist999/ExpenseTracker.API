using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<Expense> GetExpense(Guid expenseId);
        Task<Expense> AddExpense(Expense expense);
        Task<List<Expense>> GetAllExpensesByYearAndMonth(Guid accountId, ExpenseParams param, string shortMonth);
        Task<List<Expense>> GetExpensesAndPage(Guid accountId, ExpenseParams param, string shortMonth);
        Task<Expense> DeleteExpense(Guid expenseId);
    }
}
