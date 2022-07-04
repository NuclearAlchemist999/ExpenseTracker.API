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

            if (_params.Month == null && _params.Year == null && _params.Categories == null &&
                _params.StartDate == null && _params.EndDate == null && _params.SearchQuery == null)
            {
                shortMonth = DateTime.Now.ToString().ToShortMonth();
                totalExpenses = await _expenseRepo.GetExpensesByDefault(accountId, _params, shortMonth, false);
                expenses = await _expenseRepo.GetExpensesByDefault(accountId, _params, shortMonth, true);
            }

            if (_params.Month != null && _params.Year != null)
            {
                shortMonth = _params.Month.ToShortMonth();
                totalExpenses = await _expenseRepo.GetExpensesByYearAndMonth(accountId, _params, shortMonth, false);
                expenses = await _expenseRepo.GetExpensesByYearAndMonth(accountId, _params, shortMonth, true);
            }

            if (_params.Categories != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByCategories(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByCategories(accountId, _params, true);
            }

            if (_params.StartDate != null && _params.EndDate != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByTimeInterval(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByTimeInterval(accountId, _params, true);
            }

            if (_params.Categories != null && _params.StartDate != null && _params.EndDate != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByTimeIntervalAndCategories(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByTimeIntervalAndCategories(accountId, _params, true);
            }
            if (_params.Year != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByYear(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByYear(accountId, _params, true);
            }
            if (_params.Month != null)
            {
                shortMonth = _params.Month.ToShortMonth();
                totalExpenses = await _expenseRepo.GetExpensesByMonth(accountId, _params, shortMonth, false);
                expenses = await _expenseRepo.GetExpensesByMonth(accountId, _params, shortMonth, true);
            }
            if (_params.Month != null && _params.Year != null && _params.Categories != null)
            {
                shortMonth = _params.Month.ToShortMonth();
                totalExpenses = await _expenseRepo.GetExpensesByMonthYearAndCategories(accountId, _params, shortMonth, false);
                expenses = await _expenseRepo.GetExpensesByMonthYearAndCategories(accountId, _params, shortMonth, true);
            }
            if (_params.Year != null && _params.Categories != null)
            {
                totalExpenses = await _expenseRepo.GetExpensesByYearAndCategories(accountId, _params, false);
                expenses = await _expenseRepo.GetExpensesByYearAndCategories(accountId, _params, true);
            }
            if (_params.Month != null && _params.Categories != null)
            {
                shortMonth = _params.Month.ToShortMonth();
                totalExpenses = await _expenseRepo.GetExpensesByMonthAndCategories(accountId, _params, shortMonth, false);
                expenses = await _expenseRepo.GetExpensesByMonthAndCategories(accountId, _params, shortMonth, true);
            }
            if (_params.SearchQuery != null)
            {
                totalExpenses = await _expenseRepo.SearchExpenses(accountId, _params, false);
                expenses = await _expenseRepo.SearchExpenses(accountId, _params, true);
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

        public List<string> ValidateFilterParams(ExpenseParams _params)
        {
            var errors = new List<string>();

            if (_params.StartDate != null || _params.EndDate != null)
            {
                if (_params.StartDate != null && _params.EndDate == null || _params.StartDate == null &&
                    _params.EndDate != null)
                {
                    errors.Add("Both start date and end date have to be combined.");
                }
                if (DateTime.Parse(_params.StartDate) > DateTime.Parse(_params.EndDate))
                {
                    errors.Add("Start date cannot be larger than end date.");
                }
                if (_params.Year != null || _params.Month != null)
                {
                    errors.Add("Month or year cannot be combined with start date or end date.");
                }
            }
            if (_params.Limit == null || _params.Page == null)
            {
                errors.Add("Limit and page have to be entered.");
            }

            if (_params.SearchQuery != null)
            {
                if (_params.Month != null || _params.Year != null || _params.Categories != null ||
                     _params.StartDate != null || _params.EndDate != null)
                {
                    errors.Add("Search cannot be combined with other parameters.");
                }
            }

            return errors;
        }
    }
}
