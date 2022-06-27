using ExpenseTracker.API.Data;
using ExpenseTracker.API.DTO.Request;
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

        public async Task<Expense> GetExpense(Guid expenseId)
        {
            return await _exTrackContext.Expenses
                        .Include(e => e.Category)
                        .FirstOrDefaultAsync(e => e.Id == expenseId);
        }

        public async Task<Expense> AddExpense(Expense expense)
        {   
           _exTrackContext.Expenses.Add(expense);

            var category = await _exTrackContext.Categories
                         .Where(cat => cat.Id == expense.CategoryId)
                         .FirstOrDefaultAsync(); 
                        
            expense.Category.Title = category.Title;

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
                           .Include(exp => exp.Category)
                           .Sort(param.OrderBy)
                           .Select(exp => exp)
                           .Skip((param.Limit * (param.Page - 1)))
                           .Take(param.Limit)
                           .ToListAsync();

            return expenses;
        }

        public async Task<bool> DeleteExpense(Guid expenseId)
        {
            var expense = await GetExpense(expenseId);

            _exTrackContext.Expenses.Remove(expense);

            return await _exTrackContext.SaveChangesAsync() > 0;
        }

        public async Task<Expense> UpdateExpense(Guid id, UpdateExpenseRequestDto request)
        {
            var expense = await GetExpense(id);

            expense.Title = request.Title;
            expense.CategoryId = Guid.Parse(request.CategoryId);
            expense.Price = request.Price;
            expense.CreatedAt = request.CreatedAt;
            expense.CreatedYear = DateTime.Parse(request.CreatedAt).Year;
            expense.ShortMonth = request.CreatedAt.ToShortMonth();
            expense.UpdatedAt = DateTime.UtcNow;

            await _exTrackContext.SaveChangesAsync();

            return expense;
        }
    }
}
