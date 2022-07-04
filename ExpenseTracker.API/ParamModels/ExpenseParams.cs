namespace ExpenseTracker.API.ParamModels
{
    public class ExpenseParams
    {
        public int? Year { get; set; }
        public string Month { get; set; }
        public string OrderBy { get; set; }
        public int? Limit { get; set; }
        public int? Page { get; set; }
        public string Categories { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SearchQuery { get; set; } 
    }
}
