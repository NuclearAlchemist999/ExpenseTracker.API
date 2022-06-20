using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExTrackerDbContext _exTrackContext;
        public ExpenseRepository(ExTrackerDbContext exTrackContext)
        {
            _exTrackContext = exTrackContext;
        }

        public async Task<Expense> AddExpense(Expense expense)
        {
            _exTrackContext.Expenses.Add(expense);
            
            await _exTrackContext.SaveChangesAsync();

            return expense;
        }
    }
}
