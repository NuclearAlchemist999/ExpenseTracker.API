using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<Expense> AddExpense(Expense expense);
    }
}
