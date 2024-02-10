using System.Linq;
using System.Threading.Tasks;
using System;
using VirtualBazaar.Core.Models.Foundations.Orders;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Order> InsertOrderAsync(Order order);
        ValueTask<Order> UpdateOrderAsync(Order order);
        IQueryable<Order> SelectAllOrders();
        ValueTask<Order> DeleteOrderByIdAsync(Order order);
    }
}
