namespace ExpenseTracker.API.Models
{
    public class Expense
    {
        public Guid Id { get; set; }
        public Account Account { get; set; }
        public Guid AccountId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public int CreatedYear { get; set; } 
        public string ShortMonth { get; set; } 
    }
}
