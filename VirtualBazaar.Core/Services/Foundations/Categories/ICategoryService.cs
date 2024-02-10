using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Models.Foundations.Categories;

namespace VirtualBazaar.Core.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        ValueTask<Category> AddCategoryAsync(Category category);
        IQueryable<Category> RetrieveAllCategories();
        ValueTask<Category> RemoveCategoryAsync(Category category);
    }
}
