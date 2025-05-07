using System;
using System.Collections.Generic;

namespace Aplicatie.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Telephone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual List<UserAddress> Addresses { get; set; } = new();
        public virtual List<UserPayment> Payments { get; set; } = new();
        public ShoppingSession? ShoppingSession { get; set; }
        public virtual OrderDetails? OrderDetail { get; set; }

    }
}