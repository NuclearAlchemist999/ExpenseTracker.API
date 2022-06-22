using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Extensions
{
    public static class ExpenseExtensions
    {
        public static IQueryable<Expense> Sorting(this IQueryable<Expense> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderByDescending(e => e.CreatedAt);

            query = orderBy switch
            {
                "dateAsc" => query.OrderBy(e => e.CreatedAt),
                "dateDesc" => query.OrderByDescending(e => e.CreatedAt),
                _ => query.OrderByDescending(e => e.CreatedAt)
            };

            return query;
        }
    }
}
