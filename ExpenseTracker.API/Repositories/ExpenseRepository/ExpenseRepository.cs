using ExpenseTracker.API.Data;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Extensions;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;
using ExpenseTracker.API.Repositories.CategoryRepository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.API.Repositories.ExpenseRepository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExTrackerDbContext _exTrackContext;
        private readonly ICategoryRepository _categoryRepo;
        public ExpenseRepository(ExTrackerDbContext exTrackContext, ICategoryRepository categoryRepo)
        {
            _exTrackContext = exTrackContext;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Expense>> GetExpenses(Guid accountId)
        {
            return await _exTrackContext.Expenses
                        .Include(e => e.Category)
                        .Where(e => e.AccountId == accountId)
                        .ToListAsync();
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

            var category = await _categoryRepo.GetCategory(expense.CategoryId); 
                        
            expense.Category = category;

            await _exTrackContext.SaveChangesAsync();

            return expense;
        }

        public async Task<List<Expense>> GetAllExpensesByYearAndMonth(Guid accountId, ExpenseParams _params, 
            string shortMonth)
        {
            var expenses = await GetExpenses(accountId);

            return expenses.Where(e => YearAndMonth(e, _params.Year, shortMonth)).ToList();    
        }

        public async Task<List<Expense>> GetExpensesAndPage(Guid accountId, ExpenseParams _params,
            string shortMonth)
        {
            var expenses = await GetExpensesAndSort(accountId, _params);

            return expenses.Where(e => YearAndMonth(e, _params.Year, shortMonth)).ToList();       
        }
        public bool YearAndMonth(Expense expense, int year, string shortMonth)
        {
            return expense.CreatedYear == year && expense.ShortMonth == shortMonth;
        }
        public async Task<List<Expense>> GetExpensesAndSort(Guid accountId, ExpenseParams _params)
        {   
            return await _exTrackContext.Expenses
                                   .Include(e => e.Category)
                                   .Where(e => e.AccountId == accountId)
                                   .Sort(_params.OrderBy)
                                   .Skip((_params.Limit * (_params.Page - 1)))
                                   .Take(_params.Limit)
                                   .ToListAsync();
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

        public async Task<List<Expense>> GetExpensesByCategories(Guid accountId, FilterExpenseParams _params)
        {
            var expenses = await GetExpenses(accountId);
            return expenses.Where(e => _params.Categories.Contains(e.Category.Title)).ToList();
        }

        public async Task<List<Expense>> GetExpensesByTimeInterval(Guid accountId, FilterExpenseParams _params)
        {
            var startDate = DateTime.Parse(_params.StartDate);
            var endDate = DateTime.Parse(_params.EndDate);

            var expenses = await GetExpenses(accountId);

            return expenses.Where(e => DateTime.Parse(e.CreatedAt) >= startDate && 
            DateTime.Parse(e.CreatedAt) <= endDate).ToList();
        }
    }
}
