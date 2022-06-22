using System.Globalization;

namespace ExpenseTracker.API.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ConvertToDate(this string createdAt)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(createdAt, "yyyy-MM-dd",
               CultureInfo.InvariantCulture), DateTimeKind.Utc); 
        }
    }
}
