using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ExTrackerDbContext _exTrackContext;
        public CategoryRepository(ExTrackerDbContext exTrackContext)
        {
            _exTrackContext = exTrackContext;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _exTrackContext.Categories.ToListAsync();
        }
    }
}
