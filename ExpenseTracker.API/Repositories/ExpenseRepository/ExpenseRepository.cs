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

        public async Task<List<Expense>> GetExpenses(Guid accountId, ExpenseParams _params)
        {
            return await _exTrackContext.Expenses
                        .Include(e => e.Category)
                        .Where(e => e.AccountId == accountId)
                        .Sort(_params.OrderBy)
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

        public async Task<List<Expense>> GetExpensesByYearAndMonth(Guid accountId, ExpenseParams _params, 
            string shortMonth, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => YearAndMonth(e, (int)_params.Year, shortMonth)).Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => YearAndMonth(e, (int)_params.Year, shortMonth)).ToList();  
        }

        public bool YearAndMonth(Expense expense, int year, string shortMonth)
        {
            return expense.CreatedYear == year && expense.ShortMonth == shortMonth;
        }
    
        public int Skip(ExpenseParams _params)
        {
            return (int)_params.Limit * ((int)_params.Page - 1);
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

        public async Task<List<Expense>> GetExpensesByCategories(Guid accountId, ExpenseParams _params, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => GetTitles(e, GetCategories(_params))).Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => GetTitles(e, GetCategories(_params))).ToList();
        }

        public string[] GetCategories(ExpenseParams _params)
        {
            return _params.Categories.Split(',');
        }

        public bool GetTitles(Expense expense, string[] categories)
        {
            return categories.Contains(expense.Category.Title.ToLower());
        }

        public async Task<List<Expense>> GetExpensesByTimeInterval(Guid accountId, ExpenseParams _params, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate), DateTime.Parse(_params.EndDate), e)).Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate), DateTime.Parse(_params.EndDate), e)).ToList();
        }

        public bool GetTimeInterval(DateTime startDate, DateTime endDate, Expense expense)
        {
            return DateTime.Parse(expense.CreatedAt) >= startDate &&
            DateTime.Parse(expense.CreatedAt) <= endDate;
        }

        public async Task<List<Expense>> GetExpensesByTimeIntervalAndCategories(Guid accountId, ExpenseParams _params, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate), DateTime.Parse(_params.EndDate), e) && GetTitles(e, GetCategories(_params)))
                .Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate), DateTime.Parse(_params.EndDate), e) && GetTitles(e, GetCategories(_params)))
                .ToList();
        }

        public async Task<List<Expense>> GetExpensesByYear(Guid accountId, ExpenseParams _params, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => e.CreatedYear == _params.Year).Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => e.CreatedYear == _params.Year).ToList();
        }

        public async Task<List<Expense>> GetExpensesByMonth(Guid accountId, ExpenseParams _params, string shortMonth, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => e.ShortMonth == shortMonth && e.CreatedYear == DateTime.Now.Year).Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => e.ShortMonth == shortMonth && e.CreatedYear == DateTime.Now.Year).ToList();
        }

        public async Task<List<Expense>> GetExpensesByMonthYearAndCategories(Guid accountId, ExpenseParams _params, string shortMonth, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => YearAndMonth(e, (int)_params.Year, shortMonth) && GetTitles(e, GetCategories(_params)))
                .Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => YearAndMonth(e, (int)_params.Year, shortMonth) && GetTitles(e, GetCategories(_params))).ToList();
        }

        public async Task<List<Expense>> GetExpensesByYearAndCategories(Guid accountId, ExpenseParams _params, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => e.CreatedYear == _params.Year && GetTitles(e, GetCategories(_params)))
                .Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => e.CreatedYear == _params.Year && GetTitles(e, GetCategories(_params))).ToList();
        }

        public async Task<List<Expense>> GetExpensesByMonthAndCategories(Guid accountId, ExpenseParams _params, string shortMonth, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth && GetTitles(e, GetCategories(_params)))
                .Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth && GetTitles(e, GetCategories(_params))).ToList();
        }

        public async Task<List<Expense>> GetExpensesByDefault(Guid accountId, ExpenseParams _params, string shortMonth, bool withPages)
        {
            var expenses = await GetExpenses(accountId, _params);

            return withPages
                ? expenses.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth).Skip(Skip(_params)).Take((int)_params.Limit).ToList()
                : expenses.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth).ToList();
        }
    } 
}
