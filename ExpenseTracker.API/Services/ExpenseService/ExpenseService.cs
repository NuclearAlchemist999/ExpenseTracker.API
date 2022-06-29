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

        public async Task<AllExpensesResponseDto> GetAllExpensesByYearAndMonth(ExpenseParams _params,
            string cookie)
        {
            var accountId = Guid.Parse(cookie);
            var shortMonth = _params.Month.ToShortMonth();

            var totalExpenses = await _expenseRepo.GetAllExpensesByYearAndMonth(accountId, _params, shortMonth);
            var expenses = await _expenseRepo.GetExpensesAndPage(accountId, _params, shortMonth);
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

        public async Task<List<ExpenseDto>> FilterExpenses(string cookie, FilterExpenseParams _params)
        {
            var expenses = new List<Expense>();
            var accountId = Guid.Parse(cookie);

            if (_params.StartDate == null && _params.EndDate == null && _params.Month == null && _params.Year == null)
            {
                expenses = await _expenseRepo.GetExpensesByCategories(accountId, _params);
            }

            if (_params.Categories == null && _params.Month == null && _params.Year == null)
            {
                expenses = await _expenseRepo.GetExpensesByTimeInterval(accountId, _params);
            }

            return expenses.ToExpenseDtoList();
        }
    }
}
