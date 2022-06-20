using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.DTO.Converters
{
    public static class ExpenseConverter
    {
        public static ExpenseDto ToExpenseDto(this Expense expense)
        {
            return new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Category = expense.Category,
                Price = expense.Price,
                CreatedAt = expense.CreatedAt.ToString().Substring(0, 10),
                CreatedYear = expense.CreatedYear,
                ShortMonth = expense.ShortMonth
            };
        }
    }
}
