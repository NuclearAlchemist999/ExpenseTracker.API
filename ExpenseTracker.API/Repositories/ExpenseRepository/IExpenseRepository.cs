using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<Expense> AddExpense(Expense expense);
        Task<List<Expense>> GetAllExpensesByYearAndMonth(Guid accountId, ExpenseParams param, string shortMonth);
        Task<List<Expense>> GetExpensesAndPage(Guid accountId, ExpenseParams param, string shortMonth);
    }
}
