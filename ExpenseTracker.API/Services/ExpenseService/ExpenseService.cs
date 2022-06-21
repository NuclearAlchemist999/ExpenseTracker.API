using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.Repositories.ExpenseRepository;
using System.Globalization;

namespace ExpenseTracker.API.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        public ExpenseService(IExpenseRepository expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }

        public async Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, Guid accountId)
        {
            var newExpense = new Expense
            {
                AccountId = accountId,
                Title = request.Title,
                Category = request.Category,
                Price = request.Price,
                CreatedAt = DateTime.SpecifyKind(DateTime.ParseExact(request.CreatedAt, "yyyy-MM-dd", 
                CultureInfo.InvariantCulture), DateTimeKind.Utc),
                ShortMonth = GetMonth(request.CreatedAt)
            };

            var expense = await _expenseRepo.AddExpense(newExpense);

            return expense.ToExpenseDto();
        }

        public async Task<AllExpensesResponseDto> GetAllExpensesByYearAndMonth(string month, string year, string cookie)
        {
            var accountId = Guid.Parse(cookie);
            var shortMonth = GetMonth(month);
            int intYear = Int32.Parse(year);

            var expenses = await _expenseRepo.GetAllExpensesByYearAndMonth(accountId, intYear, shortMonth);

            var expenseValues = new AllExpensesResponseDto
            {
                Expenses = expenses.ToExpenseDtoList(),
                NumberOfExpenses = expenses.Count(),
                TotalCost = (decimal)expenses.Sum(e => e.Price)        
            };  

            return expenseValues;
        }

        public string GetMonth(string date)
        {
            string getMonth = "";
            string month = "";

            if (date.Length > 2)
            {
                getMonth = date.Substring(5, 2);
            }

            if (date.Length == 2)
            {
                getMonth = date;
            }
            
            switch(getMonth)
            {
                case "01":
                    month = "Jan";
                    break;
                case "02":
                    month = "Feb";
                    break;
                case "03":
                    month = "Mar";
                    break;
                case "04":
                    month = "Apr";
                    break;
                case "05":
                    month = "Maj";
                    break;
                case "06":
                    month = "Jun";
                    break;
                case "07":
                    month = "Jul";
                    break;
                case "08":
                    month = "Aug";
                    break;
                case "09":
                    month = "Sep";
                    break;
                case "10":
                    month = "Okt";
                    break;
                case "11":
                    month = "Nov";
                    break;
                case "12":
                    month = "Dec";
                    break;
                default:
                    month = "";
                    break;
            }

            return month;
        }
    }
}
