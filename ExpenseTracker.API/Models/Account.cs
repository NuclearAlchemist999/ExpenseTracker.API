namespace ExpenseTracker.API.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Theme { get; set; } = "Default";
    }
}
