using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using VirtualBazaar.Core.Models.Foundations.Orders;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Order> Orders { get; set; }

        public async ValueTask<Order> InsertOrderAsync(Order order) =>
            await InsertAsync(order);

        public IQueryable<Order> SelectAllOrders() =>
            SelectAll<Order>();

        public async ValueTask<Order> UpdateOrderAsync(Order order) =>
            await UpdateAsync(order);

        public async ValueTask<Order> DeleteOrderByIdAsync(Order order) =>
            await DeleteAsync(order);
    }
}
