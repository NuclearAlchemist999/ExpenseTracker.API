using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.Extensions;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;
using ExpenseTracker.API.Repositories.ExpenseRepository;

namespace ExpenseTracker.API.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        public ExpenseService(IExpenseRepository expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }

        public async Task<Expense> GetExpense(Guid expenseId)
        {
            return await _expenseRepo.GetExpense(expenseId);
        }
        public async Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, string cookie)
        {
            var newExpense = new Expense
            {
                AccountId = Guid.Parse(cookie),
                Title = request.Title,
                CategoryId = Guid.Parse(request.CategoryId),
                Price = request.Price,
                CreatedAt = request.CreatedAt,
                CreatedYear = DateTime.Parse(request.CreatedAt).Year,
                ShortMonth = request.CreatedAt.ToShortMonth()
            };

            var expense = await _expenseRepo.AddExpense(newExpense);

            return expense.ToExpenseDto();
        }

        public async Task<AllExpensesResponseDto> FilterExpenses(ExpenseParams _params,
            string cookie)
        {
            var accountId = Guid.Parse(cookie);

            string shortMonth = "";

            var totalExpenses = new List<Expense>();
            var expenses = new List<Expense>();

            if (_params.Month != null && _params.Year != null && _params.Categories == null &&
                _params.StartDate == null && _params.EndDate == null)
            {
                shortMonth = _params.Month.ToShortMonth();
                totalExpenses = await _expenseRepo.GetExpensesByYearAndMonth(accountId, _params, shortMonth, false);
                expenses = await _expenseRepo.GetExpensesByYearAndMonth(accountId, _params, shortMonth, true);
            }

            if (_params.Month == null && _params.Year == null && _params.Categories != null &&
                _params.StartDate == null && _params.EndDate == null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByCategories(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByCategories(accountId, _params, true);
            }

            if (_params.Month == null && _params.Year == null && _params.Categories == null &&
                _params.StartDate != null && _params.EndDate != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByTimeInterval(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByTimeInterval(accountId, _params, true);
            }

            if (_params.Month == null && _params.Year == null && _params.Categories != null &&
                _params.StartDate != null && _params.EndDate != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByTimeIntervalAndCategories(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByTimeIntervalAndCategories(accountId, _params, true);
            }
            if (_params.Month == null && _params.Year != null && _params.Categories == null &&
                _params.StartDate == null && _params.EndDate == null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByYear(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByYear(accountId, _params, true);
            }

            double totalPages = Math.Ceiling((double)totalExpenses.Count() / (double)_params.Limit);

            var expenseValues = new AllExpensesResponseDto
            {
                Expenses = expenses.ToExpenseDtoList(),
                NumberOfExpenses = totalExpenses.Count(),
                TotalCost = (decimal)totalExpenses.Sum(e => e.Price),
                TotalPages = (int)totalPages
            };

            return expenseValues;
          
        }

        public async Task<bool> DeleteExpense(Guid id)
        {
            return await _expenseRepo.DeleteExpense(id);
        }

        public async Task<Expense> UpdateExpense(Guid id, UpdateExpenseRequestDto request)
        {
            return await _expenseRepo.UpdateExpense(id, request);
        }
    }
}
