using System;
using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Models.Foundations.Products;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Product> InsertProductAsync(Product product);
        ValueTask<Product> UpdateProductAsync(Product product);
        ValueTask<Product> SelectProductByIdAsync(Guid id);
        IQueryable<Product> SelectAllProducts();
        ValueTask<Product> DeleteProductAsync(Product product);
    }
}
