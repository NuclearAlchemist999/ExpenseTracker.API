namespace ExpenseTracker.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToShortMonth(this string createdAt)
        {
            string getMonth = createdAt;
            string month = "";

            if (createdAt.Length > 2)
            {
                getMonth = createdAt.Substring(5, 2);
            }

            if (createdAt.Length == 2)
            {
                getMonth = createdAt;
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

