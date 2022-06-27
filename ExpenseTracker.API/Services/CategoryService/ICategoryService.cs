using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
    }
}
