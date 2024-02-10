using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Brokers.Loggings;
using VirtualBazaar.Core.Brokers.Storages;
using VirtualBazaar.Core.Models.Foundations.Categories;

namespace VirtualBazaar.Core.Services.Foundations.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public CategoryService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Category> AddCategoryAsync(Category category) =>
            await this.storageBroker.InsertCategoryAsync(category);

        public IQueryable<Category> RetrieveAllCategories() =>
            this.storageBroker.SelectAllCategories();

        public async ValueTask<Category> RemoveCategoryAsync(Category category) =>
            await this.storageBroker.DeleteCategoryByIdAsync(category);
    }
}
