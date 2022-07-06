namespace ExpenseTracker.API.Exceptions
{
    public sealed class UsernameTakenException : BadRequestException
    {
        public UsernameTakenException()
            : base("Username already exists.")
        {
        }
    }
}
