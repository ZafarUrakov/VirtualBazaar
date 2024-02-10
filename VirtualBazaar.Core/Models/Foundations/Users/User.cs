using System;
using System.Collections;
using System.Collections.Generic;
using VirtualBazaar.Core.Models.Foundations.Orders;

namespace VirtualBazaar.Core.Models.Foundations.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public long TelegramId { get; set; }
        public Guid HelperId { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}
