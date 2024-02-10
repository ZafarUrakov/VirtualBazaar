using System.Linq;
using System.Threading.Tasks;
using System;
using VirtualBazaar.Core.Models.Foundations.Products;

namespace VirtualBazaar.Core.Services.Foundations.Products
{
    public interface IProductService
    {
        ValueTask<Product> AddProductAsync(Product product);
        ValueTask<Product> RetrieveProductWithTeacherByIdAsync(Guid productId);
        IQueryable<Product> RetrieveAllProducts();
        ValueTask<Product> ModifyProductAsync(Product product);
        ValueTask<Product> RemoveProductAsync(Product product);
    }
}
