using System;

namespace Aplicatie.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ShoppingSession Session { get; set; }
        public Product Product { get; set; }
    }
}
