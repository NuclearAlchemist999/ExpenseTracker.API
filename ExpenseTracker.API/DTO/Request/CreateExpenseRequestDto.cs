namespace ExpenseTracker.API.DTO.Request
{
    public class CreateExpenseRequestDto
    {   
        public string Title { get; set; }
        public string CategoryId { get; set; }
        public double Price { get; set; }
        public string CreatedAt { get; set; }
    }
}
