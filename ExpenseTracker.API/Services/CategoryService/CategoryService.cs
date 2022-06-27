using ExpenseTracker.API.Models;
using ExpenseTracker.API.Repositories.CategoryRepository;

namespace ExpenseTracker.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _categoryRepo.GetCategories();
        }
    }
}
