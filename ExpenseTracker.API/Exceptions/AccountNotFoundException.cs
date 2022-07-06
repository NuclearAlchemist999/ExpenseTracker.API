namespace ExpenseTracker.API.Exceptions
{
    public sealed class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException()
            : base("Account does not exist.")
        {
        }
    }
}
