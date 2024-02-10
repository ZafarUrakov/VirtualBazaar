using System;
using System.Collections.Generic;
using VirtualBazaar.Core.Models.Foundations.Admins;
using VirtualBazaar.Core.Models.Foundations.Products;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Models.Foundations.Categories
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid AdminId { get; set; }
    }
}
