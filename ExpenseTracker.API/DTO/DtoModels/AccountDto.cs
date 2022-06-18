namespace ExpenseTracker.API.DTO.DtoModels
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Theme { get; set; } = "Default";
    }
}
