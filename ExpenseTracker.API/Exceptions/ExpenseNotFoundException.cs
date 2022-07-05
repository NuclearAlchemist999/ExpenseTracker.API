namespace ExpenseTracker.API.Exceptions
{
    public sealed class ExpenseNotFoundException : NotFoundException
    {
        public ExpenseNotFoundException(Guid expenseId)
            : base ($"The expense with id {expenseId} does not exist.")
        {

        }
    }
}
