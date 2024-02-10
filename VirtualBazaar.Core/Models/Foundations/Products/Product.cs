using System;
using VirtualBazaar.Core.Models.Foundations.Admins;
using VirtualBazaar.Core.Models.Foundations.Categories;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Models.Foundations.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string PhotoUrl { get; set; }
        public Guid AdminId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
