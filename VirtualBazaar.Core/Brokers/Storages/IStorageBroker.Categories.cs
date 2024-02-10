using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Models.Foundations.Categories;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Category> InsertCategoryAsync(Category category);
        IQueryable<Category> SelectAllCategories();
        ValueTask<Category> DeleteCategoryByIdAsync(Category category);
    }
}
