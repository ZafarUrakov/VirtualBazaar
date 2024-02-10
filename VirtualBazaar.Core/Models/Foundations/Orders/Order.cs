using System;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Models.Foundations.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public Guid UserId { get; set; }
    }
}
