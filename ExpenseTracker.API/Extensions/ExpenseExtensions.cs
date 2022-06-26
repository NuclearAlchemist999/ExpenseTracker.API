using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Extensions
{
    public static class ExpenseExtensions
    {
        public static IQueryable<Expense> Sort(this IQueryable<Expense> query, string orderBy)
        {
            var orderByDesc = query.OrderByDescending(e => e.CreatedAt);
           

            if (string.IsNullOrWhiteSpace(orderBy)) return orderByDesc;

            //var year = DateTime.Parse("2022-06-22").Year;
            //var month = DateTime.Parse("2022-06-22").Month;
            //var day = DateTime.Parse("2022-06-22").Day;

            //var dateString = "2022-06-22";

        //    var fuckyoudateIhateYou = Convert.ToDateTime("2022-06-22");

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

