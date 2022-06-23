using ExpenseTracker.API.Data;
using ExpenseTracker.API.Extensions;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;
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

        public async Task<List<Expense>> GetAllExpensesByYearAndMonth(Guid accountId, ExpenseParams param, 
            string shortMonth)
        {
            var expenses = await _exTrackContext.Expenses
               .Where(exp => exp.CreatedYear == param.Year && exp.ShortMonth == shortMonth && 
                exp.AccountId == accountId)
               .ToListAsync();
                
            return expenses;        
        }

        public async Task<List<Expense>> GetExpensesAndPage(Guid accountId, ExpenseParams param,
            string shortMonth)
        {
            var expenses = await _exTrackContext.Expenses
               .Where(exp => exp.CreatedYear == param.Year && exp.ShortMonth == shortMonth &&
                exp.AccountId == accountId)
               .Sort(param.OrderBy)
               .Select(exp => exp)
               .Skip(param.Limit * (param.Page - 1))
               .Take(param.Limit)
               .ToListAsync();

            return expenses;
        }
    }
}
