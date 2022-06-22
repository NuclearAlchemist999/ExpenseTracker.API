using ExpenseTracker.API.Models;
using System.Globalization;

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

        public static DateTime ConvertToDate(this Expense expense, string date)
        {
            return expense.CreatedAt = DateTime.SpecifyKind(DateTime.ParseExact(date, "yyyy-MM-dd",
               CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }

        public static string GetMonth(this Expense expense, string date)
        {
            string getMonth = "";
            string month = expense.ShortMonth;

            if (date.Length > 2)
            {
                getMonth = date.Substring(5, 2);
            }

            if (date.Length == 2)
            {
                getMonth = date;
            }

            switch (getMonth)
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

