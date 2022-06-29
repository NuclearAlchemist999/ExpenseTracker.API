namespace ExpenseTracker.API.ParamModels
{
    public class FilterExpenseParams
    {
        public int? Year { get; set; }
        public string Month { get; set; }
        public List<string>? Categories { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
