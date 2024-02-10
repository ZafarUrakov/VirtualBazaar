using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtualBazaar.Core.Models.Foundations.Categories;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Category> Categories { get; set; }

        public async ValueTask<Category> InsertCategoryAsync(Category category) =>
           await InsertAsync(category);

        public IQueryable<Category> SelectAllCategories() =>
            SelectAll<Category>();

        public async ValueTask<Category> DeleteCategoryByIdAsync(Category category) =>
            await DeleteAsync(category);
    }
}
