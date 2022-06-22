using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.Extensions;
using ExpenseTracker.API.Models;
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

        public async Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, string cookie)
        {
            var accountId = Guid.Parse(cookie);

            var newExpense = new Expense
            {
                AccountId = accountId,
                Title = request.Title,
                Category = request.Category,
                Price = request.Price,
                CreatedAt = request.CreatedAt.ConvertToDate(),
                ShortMonth = request.CreatedAt.ToShortMonth()
            };

            var expense = await _expenseRepo.AddExpense(newExpense);

            return expense.ToExpenseDto();
        }

        public async Task<AllExpensesResponseDto> GetAllExpensesByYearAndMonth(string month, string year, 
            string cookie, string orderBy)
        {
            var accountId = Guid.Parse(cookie);
            var shortMonth = month.ToShortMonth();
            int intYear = int.Parse(year);

            var expenses = await _expenseRepo.GetAllExpensesByYearAndMonth(accountId, intYear, shortMonth, orderBy);

            var expenseValues = new AllExpensesResponseDto
            {
                Expenses = expenses.ToExpenseDtoList(),
                NumberOfExpenses = expenses.Count(),
                TotalCost = (decimal)expenses.Sum(e => e.Price)        
            };  

            return expenseValues;
        }
    }
}
