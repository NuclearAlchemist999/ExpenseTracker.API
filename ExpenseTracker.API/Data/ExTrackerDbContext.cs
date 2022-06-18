using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Data
{
    public class ExTrackerDbContext : DbContext
    {
        public ExTrackerDbContext(DbContextOptions<ExTrackerDbContext> options) : base(options) 
        {
        }
    }
}
