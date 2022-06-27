using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
    }
}
