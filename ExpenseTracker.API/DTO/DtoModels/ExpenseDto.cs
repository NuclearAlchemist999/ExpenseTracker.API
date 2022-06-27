using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.DTO.DtoModels
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public string CreatedAt { get; set; } 
        public int CreatedYear { get; set; } 
        public string ShortMonth { get; set; }
    }
}
