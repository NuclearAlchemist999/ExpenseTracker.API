namespace ExpenseTracker.API.DTO.Response
{
    public class ValidateFilterResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
