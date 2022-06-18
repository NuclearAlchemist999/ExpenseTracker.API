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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int CreatedYear { get; set; } = DateTime.Today.Year;
        public string ShortMonth { get; set; } 
    }
}
