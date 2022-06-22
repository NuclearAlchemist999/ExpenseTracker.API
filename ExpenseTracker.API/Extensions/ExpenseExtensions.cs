using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Extensions
{
    public static class ExpenseExtensions
    {
        public static IQueryable<Expense> Sort(this IQueryable<Expense> query, string orderBy)
        {
            var orderByDesc = query.OrderByDescending(e => e.CreatedAt);

            if (string.IsNullOrWhiteSpace(orderBy)) return orderByDesc;

            query = orderBy switch
            {
                "dateAsc" => query.OrderBy(e => e.CreatedAt),
                "dateDesc" => orderByDesc,
                _ => orderByDesc
            };

            return query;
        }
    }
}

