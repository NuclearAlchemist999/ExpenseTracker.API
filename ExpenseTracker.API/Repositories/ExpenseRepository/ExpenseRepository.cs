using ExpenseTracker.API.Data;
using ExpenseTracker.API.Extensions;
using ExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Expense>> GetAllExpensesByYearAndMonth(Guid accountId, int createdYear, 
            string shortMonth, string orderBy)
        {
            var expenses = await _exTrackContext.Expenses
               .Where(exp => exp.CreatedYear == createdYear && exp.ShortMonth == shortMonth && 
                exp.AccountId == accountId)
               .Sorting(orderBy)
               .Select(exp => exp)
               .ToListAsync();
                
            return expenses;        
        }
    }
}
