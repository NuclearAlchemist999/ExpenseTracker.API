namespace ExpenseTracker.API.Exceptions
{
    public sealed class InvalidCredentialsException : BadRequestException
    {
        public InvalidCredentialsException()
            : base("Invalid credentials.")
        {
        }
    }
}
