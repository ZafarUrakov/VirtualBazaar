using System.Linq;
using System.Threading.Tasks;
using System;
using VirtualBazaar.Core.Brokers.Loggings;
using VirtualBazaar.Core.Brokers.Storages;
using VirtualBazaar.Core.Models.Foundations.Products;

namespace VirtualBazaar.Core.Services.Foundations.Products
{
    public class ProductService : IProductService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProductService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Product> AddProductAsync(Product product) =>
            await this.storageBroker.InsertProductAsync(product);

        public async ValueTask<Product> RetrieveProductWithTeacherByIdAsync(Guid productId) =>
            await this.storageBroker.SelectProductByIdAsync(productId);

        public IQueryable<Product> RetrieveAllProducts() =>
            this.storageBroker.SelectAllProducts();

        public async ValueTask<Product> ModifyProductAsync(Product product) =>
            await this.storageBroker.UpdateProductAsync(product);

        public async ValueTask<Product> RemoveProductAsync(Product product) =>
            await this.storageBroker.DeleteProductAsync(product);
    }
}
