using ExpenseTracker.API.Models;
using System.Globalization;
using System.Text;

namespace ExpenseTracker.API.Extensions
{
    public static class ExpenseExtensions
    {
        public static IQueryable<Expense> Sort(this IQueryable<Expense> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderByDescending(e => e.CreatedAt);

            query = orderBy switch
            {
                "dateAsc" => query.OrderBy(e => e.CreatedAt),
                "dateDesc" => query.OrderByDescending(e => e.CreatedAt),
                "categoryAsc" => query.OrderBy(e => e.Category.Title),
                "categoryDesc" => query.OrderByDescending(e => e.Category.Title),
                "titleAsc" => query.OrderBy(e => e.Title),
                "titleDesc" => query.OrderByDescending(e => e.Title),
                "priceAsc" => query.OrderBy(e => e.Price),
                "priceDesc" => query.OrderByDescending(e => e.Price),
                _ => query.OrderByDescending(e => e.CreatedAt)
            };

            return query;
        }
    }
}

