using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Brokers.Loggings;
using VirtualBazaar.Core.Brokers.Storages;
using VirtualBazaar.Core.Models.Foundations.Orders;

namespace VirtualBazaar.Core.Services.Foundations.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public OrderService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Order> AddOrderAsync(Order order) =>
            await this.storageBroker.InsertOrderAsync(order);

        public IQueryable<Order> RetrieveAllOrders() =>
            this.storageBroker.SelectAllOrders();

        public async ValueTask<Order> ModifyOrderAsync(Order order) =>
            await this.storageBroker.UpdateOrderAsync(order);

        public async ValueTask<Order> RemoveOrderAsync(Order order) =>
            await this.storageBroker.DeleteOrderByIdAsync(order);
    }
}
