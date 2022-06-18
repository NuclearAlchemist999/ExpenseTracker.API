using ExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Data
{
    public class ExTrackerDbContext : DbContext
    {
        public ExTrackerDbContext(DbContextOptions<ExTrackerDbContext> options) : base(options) 
        {
        }
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Expense> Expenses => Set<Expense>();
    }
}
